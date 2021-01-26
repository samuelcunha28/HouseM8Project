using Enums;
using HouseM8API.Models;
using HouseM8API.Models.ReturnedMessages;
using Microsoft.AspNetCore.Http;
using Models;
using System;
using System.Collections.Generic;

namespace HouseM8API.Interfaces
{
    /// <summary>
    /// Interface para efetuar operações na base de dados
    /// relativas ao Job
    /// </summary>
    public interface IJobDAO : IDisposable
    {
        /// <summary>
        /// Método para criar um Post 
        /// </summary>
        /// <param name="id">Id do Employer que vai criar o Post</param>
        /// <param name="model">Objeto do Post com a informação a ser publicada</param>
        /// <returns>Retorna o JobPost criado</returns>
        JobPost Create(int id, JobPost model);

        /// <summary>
        /// Metodo de pesquisa de trabalho disponível para o mate com vários filtros disponíveis
        /// </summary>
        /// <param name="categories">Filtro de Categorias</param>
        /// <param name="address">Filtro da Morada</param>
        /// <param name="distance">Filtro de distância</param>
        /// <param name="rating">Filtro de Rating</param>
        /// <param name="mateId"></param>
        /// <returns>Retorna a listagem de posts</returns>
        List<JobPostReturnedModel> GetJobs(Categories[] categories, string address, int? distance, int? rating, int mateId);

        /// <summary>
        /// Metodo para apagar um jobPost
        /// </summary>
        /// <param name="toDelete">Objeto Jobpost que vai ser apagado</param>
        /// <returns>True se for apagado, false caso contrário</returns>
        bool Delete(JobPost toDelete);

        /// <summary>
        /// Método para Encontrar um Post por Id
        /// </summary>
        /// <param name="id">Id do Post a ser encontrado</param>
        /// <returns>Retorna o JobPost procurado</returns>
        JobPost FindById(int id);

        /// <summary>
        /// Método para Encontrar um Post por Id
        /// com employer correto
        /// </summary>
        /// <param name="id">Id do jobpost</param>
        /// <param name="employerId">id do employer</param>
        /// <returns></returns>
        JobPost FindById(int id, int employerId);

        /// <summary>
        /// Método que retorna a listagem 
        /// de todos os JobPosts de um Employer
        /// </summary>
        /// <param name="employer">Id do Employer dos JobPosts</param>
        /// <returns>Retorna uma lista de Jobposts associada ao Employer</returns>
        List<JobPost> GetEmployerPosts(int employer);

        /// <summary>
        /// Metodo para realizar uma oferta de preço a um trabalho selecionado
        /// </summary>
        /// <param name="offer">Objeto Offer com informação da oferta</param>
        /// <param name="mateId">Id do Mate que realiza a oferta</param>
        /// <returns>Retorna a oferta feita pelo Mate</returns>
        Offer makeOfferOnJob(Offer offer, int? mateId);

        /// <summary>
        /// Método para atualizar os detalhes de um post
        /// </summary>
        /// <param name="model">Modelo do post com dados novos para atualizar</param>
        /// <returns>retorna o JobPost atualizado</returns>
        JobPost UpdatePostDetails(JobPost model);

        /// <summary>
        /// Método que adiciona vários
        /// tipos de pagamento a uma 
        /// publicação de trabalho
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="payments"></param>
        void AddPayment(int jobId, PaymentModel[] payments);

        /// <summary>
        /// Método para remover um tipo de pagamento
        /// de uma publicação de trabalho
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="payment"></param>
        void RemovePayment(int jobId, PaymentModel payment);

        /// <summary>
        /// Método que permite fazer upload de imagens para um post
        /// </summary>
        /// <param name="id">id do post</param>
        /// <param name="images">coleção de imagens</param>
        /// <param name="mainImage"></param>
        /// <returns>Retorna os caminhos das imagens adicionadas</returns>
        SuccessMessageModel UploadImagesToPost(int id, IFormFileCollection images, IFormFile mainImage);

        /// <summary>
        /// Método que retorna uma lista com os nomes das imagens
        /// do jobPost
        /// </summary>
        /// <param name="id">Id do post</param>
        /// <returns>Retorna lista com nomes de imagens</returns>
        List<ImageName> getImages(int id);

        /// <summary>
        /// Método que retorna o nome da imagem de destaque
        /// </summary>
        /// <param name="id">Id do post</param>
        /// <returns>Retorna o nome da Imagem de destaque</returns>
        ImageName getMainImage(int id);

        /// <summary>
        /// Método para apagar a imagem de destaque de um JobPost
        /// </summary>
        /// <param name="id">Id do employer</param>
        /// <param name="post">Id do JobPost</param>
        /// <param name="image">Objeto ImageName com o nome da imagem</param>
        /// <returns>Retorna True se a imagem for apagada, 
        /// False caso contrário</returns>
        bool deleteMainImage(int id, int post, ImageName image);

        /// <summary>
        /// Método para apagar uma imagem de um JobPost
        /// </summary>
        /// <param name="id">Id do employer</param>
        /// <param name="post">Id do JobPost</param>
        /// <param name="image">Objeto ImageName com o nome da imagem</param>
        /// <returns>Retorna True se a imagem for apagada, 
        /// False caso contrário</returns>

        bool deleteImage(int id, int post, ImageName image);
    }
}
