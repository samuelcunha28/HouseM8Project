using HouseM8API.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HouseM8API.Helpers
{   
    /// <summary>
    /// Classe helper para gerir o upload de imagens
    /// </summary>
    public class ImageUploadHelper
    {
        private static int _counter = 1;
        private static char separator = Path.DirectorySeparatorChar;

        /// <summary>
        /// Método usado para fazer upload de imagens
        /// para um post
        /// </summary>
        /// <param name="images">Coleção de imagens</param>
        /// <param name="root">diretório root do projeto</param>
        /// <param name="id">id do tipo do objeto</param>
        /// <param name="type">entidade a que se referem as imagens</param>
        /// <returns>retorna string com os paths das imagens</returns>
        public static string UploadPostImages(IFormFileCollection images, string root, int id, string type)
        {
            if(images.Count == 0)
            {
                throw new Exception("Sem Imagens!");
            }

            if(!type.Equals("post")){
                throw new Exception("Entidade não é post!");
            }

            string path = checkRoot(type, id);

            if(path == null){
                throw new Exception("Entidade/tipo inexistente!");
            }

            root += path;

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            IFormFile[] imagesArray = images.ToArray();

            for(int i = 0; i < imagesArray.Length; i++)
            {
                try
                {
                    if (imagesArray[i].Length > 0)
                    {
                        using (FileStream fileStream = File.Create(root + imagesArray[i].FileName))
                        {
                            imagesArray[i].CopyTo(fileStream);
                            fileStream.Flush();
                        }

                        string oldName = root + imagesArray[i].FileName;
                        string newName = root + i;
                        string extension = Path.GetExtension(oldName);

                        while(hasFiles(root, i + ".*")){
                            
                            int newValue = (i + _counter);
                            newName = root + newValue;

                            if(!hasFiles(root, newValue + ".*")){
                                _counter = 0;
                                break;
                            }
                            _counter++;
                        }

                        newName += extension;
                        File.Move(oldName, newName);

                    }
                    else
                    {
                        throw new Exception("Sem Imagens!");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return root;
        }

        /// <summary>
        /// Método para fazer upload de uma imagem de destaque
        /// para Post, imagem de perfil
        /// </summary>
        /// <param name="image">Imagem</param>
        /// <param name="root">Direirio do projeto</param>
        /// <param name="id">id de user/jobpost</param>
        /// <param name="type">tipo de entidade (post/user)</param>
        /// <returns>Caminho da imagem</returns>
        public static string UploadImage(IFormFile image, string root, int id, string type){

            if(image == null || image.Length == 0)
            {
                throw new Exception("Sem Imagens!");
            }

            if(type.Equals("post")){
                throw new Exception("A imagem tem de ser para postMain ou user!");
            }

            string path = checkRoot(type, id);
            

            if(path == null){
                throw new Exception("Entidade/tipo inexistente!");
            }

            root += path;

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            try{

                string oldName = root + image.FileName;
                string newName = root;
                string extension = Path.GetExtension(oldName);

                if(type.Equals("postMain")){
                    newName += "main";
                        
                    if(hasFiles(root, "main.*")){

                        string[] files = Directory.GetFiles(root, "main.*");

                        foreach (string file in files){
                            File.Delete(file);
                        }                                      
                    }

                } else if(type.Equals("user")){
                    newName += "profilePic";
                        
                    if(hasFiles(root, "profilePic.*")){

                        string[] files = Directory.GetFiles(root, "profilePic.*");

                        foreach (string file in files){
                            File.Delete(file);
                        }                                      
                    }
                } else {
                    throw new Exception("O tipo tem de ser user ou postMain!");
                }


                using (FileStream fileStream = File.Create(root + image.FileName))
                {
                    image.CopyTo(fileStream);
                    fileStream.Flush();
                }

                newName += extension;
                File.Move(oldName, newName);

                return root;

            } catch(Exception e) {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método que retorna uma lista com os nomes das imagens
        /// </summary>
        /// <param name="jobPost">Id do jobPost</param>
        /// <param name="root">Diretório do projeto</param>
        /// <param name="type">Tipo de entidade a que se referem as imagens</param>
        /// <returns>Lista com nomes de imagens</returns>
        public static List<ImageName> getImages(int jobPost, string root, string type){

            if(!type.Equals("post")){
                throw new Exception("O tipo tem de ser post!");
            }

            root += checkRoot(type, jobPost);

            try{
                string[] files = Directory.GetFiles(root,"*.*",SearchOption.TopDirectoryOnly);

                if(files.Length == 0){
                    throw new Exception("Diretório sem ficheiros!");
                }

                List<ImageName> imageList = new List<ImageName>();
                foreach(string file in files){
                    FileInfo info = new FileInfo(file);
                    string fileName = Path.GetFileName(info.FullName);
                    imageList.Add(new ImageName{Name = fileName});
                }
    
                return imageList;
            } catch {
                throw new Exception("Sem Imagens!");
            }
        }

        /// <summary>
        /// Método que retorna uma imagem de destaque ou perfil
        /// </summary>
        /// <param name="id">id do post/user</param>
        /// <param name="root">Diretório do projeto</param>
        /// <param name="type">Tipo de entidade a que se referem as imagens</param>
        /// <returns>Nome da imagem</returns>
        public static ImageName getMainImage(int id, string root, string type){

            if(!type.Equals("post")){
                root += checkRoot(type, id);
            } else {
                throw new Exception("O tipo tem de ser postMain ou user");
            }

            try{

                string[] files = Directory.GetFiles(root,"*.*",SearchOption.TopDirectoryOnly);

                if(files.Length == 0){
                    throw new Exception("Diretório sem ficheiros!");
                }

                FileInfo info = new FileInfo(files[0]);
                string fileName = Path.GetFileName(info.FullName);

                return new ImageName{Name = fileName};

            } catch {
                throw new Exception("Sem imagens!");
            }
        }

        /// <summary>
        /// Método para apagar uma imagem de JobPost
        /// </summary>
        /// <param name="id">Id do JobPost</param>
        /// <param name="name">Nome da Imagem</param>
        /// <param name="root">Diretório do projeto</param>
        /// <param name="type">Tipo de entidade a que se refere a imagem</param>
        /// <returns>Retorna True se a imagem for apagada,
        ///  False caso contrário</returns>
        public static bool deletePostImage(int id, string name, string root, string type){

            if(type.Equals("user")){
                throw new Exception("O tipo tem de ser post/postMain");
            }
            
            string path = checkRoot(type, id);

            if(path != null){
                root += path;
            } else {
                throw new Exception("O caminho não existe!");
            }

            root += name;

            if(File.Exists(root)){
                File.Delete(root);
                return true;
            } else {
                return false;
            }

        }
        
        /// <summary>
        /// Método para apagar o diretorio da imagem do user,
        /// pois só contem uma imagem
        /// </summary>
        /// <param name="id">Id do user</param>
        /// <param name="root">Diretorio do projeto</param>
        /// <param name="type">Tipo de entidade a que se refere a imagem</param>
        /// <returns>Retorna True se for apagada, 
        /// False caso contrário</returns>
        public static bool deleteUserImage(int id, string root, string type){

            if(!type.Equals("user")){
                throw new Exception("O tipo tem de ser user");
            }

            string path = checkRoot(type, id);

            if(path != null){
                root += path;
            } else {
                throw new Exception("O caminho não existe!");
            }

            if(Directory.Exists(root)){
                Directory.Delete(root,true);
                return true;
            } else {
                return false;
            }

        }

        /// <summary>
        /// Método que verifica se existem varios ficheiros
        /// com o mesmo nome, mas com extensões diferentes
        /// </summary>
        /// <param name="root">Caminho para as imagens</param>
        /// <param name="nameAndExt">Nome com extensão</param>
        /// <returns>True se existirem imagens,
        ///  False caso contrário</returns>
        private static bool hasFiles(string root, string nameAndExt){
            string[] files = Directory.GetFiles(root, nameAndExt);
            return files.Length > 0 ? true : false;
        }

        /// <summary>
        /// Verifica pra que tipo de entidade vão ser
        /// enviadas as imagens e retorna um path
        /// para o local de upload certo
        /// </summary>
        /// <param name="type">tipo de entidade a que a imagem se refere</param>
        /// <param name="id">id da entidade (user/post)</param>
        /// <returns>caminho da imagem</returns>
        private static string checkRoot(string type, int id){

            if(type.Equals("post")){
                return $"{separator}Uploads{separator}" + 
                $"Posts{separator}" + id + $"{separator}";
            }

            if(type.Equals("postMain")){
                return $"{separator}Uploads{separator}" + 
                $"Posts{separator}" + id + $"{separator}" 
                + "main" + $"{separator}";
            }

            if(type.Equals("user")){
                return $"{separator}Uploads{separator}" + 
                $"Users{separator}" + id + $"{separator}";
            }

            return null;
        }
    }
}
