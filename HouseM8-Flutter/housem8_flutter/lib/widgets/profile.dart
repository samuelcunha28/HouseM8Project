import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/widgets/counting_stars.dart';

class Profile extends StatelessWidget {
  final String role;
  final String firstName;
  final String lastName;
  final String address;
  final String bio;
  final double averageRating;

  Profile(
    this.role,
    this.firstName,
    this.lastName,
    this.address,
    this.bio,
    this.averageRating, {
    Key key,
  });

  @override
  Widget build(BuildContext context) {
    if (this.role == "MATE") {
      return Column(children: [
        Container(
          decoration: BoxDecoration(
            color: Color(0xFF1565C0),
            boxShadow: [
              BoxShadow(
                color: Colors.black12,
                blurRadius: 6.0,
                offset: Offset(0, 2),
              ),
            ],
          ),

          //Foto de perfil
          child: Container(
            width: double.infinity,
            height: 150,
            child: Container(
              alignment: Alignment(0.0, 15.0),
              child: CircleAvatar(
                backgroundImage: NetworkImage(
                    "https://icon-library.com/images/facebook-user-icon/facebook-user-icon-4.jpg"),
                radius: 70.0,
              ),
            ),
          ),
        ),

        //Nome
        SizedBox(
          height: 85,
        ),
        Text(
          this.firstName + " " + this.lastName,
          style: TextStyle(
              fontSize: 25.0,
              color: Color(0xFF2F4858),
              fontWeight: FontWeight.bold),
        ),

        //Localização
        SizedBox(
          height: 10,
        ),
        address.toString().length > 30
            ? Text(
                '' + address.substring(0, 30) + "...",
                style: TextStyle(
                    fontSize: 18.0,
                    color: Color(0xFF5B82AA),
                    fontWeight: FontWeight.w300),
              )
            : Text(
                address,
                style: TextStyle(
                    fontSize: 18.0,
                    color: Color(0xFF5B82AA),
                    fontWeight: FontWeight.w300),
              ),

        //Rating
        SizedBox(
          height: 10,
        ),
        CountingStars(this.averageRating, this.role),

        //Bio
        Container(
          child: Padding(
            padding:
                const EdgeInsets.symmetric(vertical: 30.0, horizontal: 10.0),
            child: Column(
              children: <Widget>[
                Text(
                  "Bio:",
                  style: TextStyle(
                    color: Color(0xFF5B82AA),
                    fontSize: 18.0,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                SizedBox(
                  height: 10.0,
                ),
                Text(
                  this.bio,
                  style: TextStyle(
                    fontSize: 16.0,
                    color: Color(0xFF5B82AA),
                    fontWeight: FontWeight.w300,
                  ),
                ),
              ],
            ),
          ),
        ),

        Text(
          'Redes Sociais:',
          style: TextStyle(
            fontSize: 16.0,
            color: Color(0xFF5B82AA),
            fontWeight: FontWeight.bold,
          ),
        ),
        Padding(
          padding: EdgeInsets.symmetric(vertical: 10.0),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: <Widget>[
              //Facebook
              Container(
                child: FlatButton(
                  onPressed: () => print('Facebook'),
                  //perfil de Facebook do utilizador
                  child: Container(
                    height: 60.0,
                    width: 60.0,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      color: Colors.white,
                      boxShadow: [
                        BoxShadow(
                          color: Colors.black26,
                          offset: Offset(0, 2),
                          blurRadius: 6.0,
                        ),
                      ],
                      image: DecorationImage(
                        image: AssetImage('assets/images/facebook.jpg'),
                      ),
                    ),
                  ),
                ),
              ),

              //Linkedin
              Container(
                child: FlatButton(
                  onPressed: () => print('LinkedIn'),
                  //perfil de LinkedIn do utilizador
                  child: Container(
                    height: 60.0,
                    width: 60.0,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      color: Colors.white,
                      boxShadow: [
                        BoxShadow(
                          color: Colors.black26,
                          offset: Offset(0, 2),
                          blurRadius: 6.0,
                        ),
                      ],
                      image: DecorationImage(
                        image: AssetImage('assets/images/linkedin.png'),
                      ),
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
      ]);
    } else {
      return Column(children: [
        Container(
          decoration: BoxDecoration(
            color: Color(0xFF006064),
            boxShadow: [
              BoxShadow(
                color: Colors.black12,
                blurRadius: 6.0,
                offset: Offset(0, 2),
              ),
            ],
          ),

          //Foto de perfil
          child: Container(
            width: double.infinity,
            height: 150,
            child: Container(
              alignment: Alignment(0.0, 15.0),
              child: CircleAvatar(
                backgroundImage: NetworkImage(
                    "https://icon-library.com/images/facebook-user-icon/facebook-user-icon-4.jpg"),
                radius: 70.0,
              ),
            ),
          ),
        ),

        //Nome
        SizedBox(
          height: 85,
        ),
        Text(
          this.firstName + " " + this.lastName,
          style: TextStyle(
              fontSize: 25.0,
              color: Color(0xFF00171F),
              fontWeight: FontWeight.bold),
        ),
        //Localização
        SizedBox(
          height: 10,
        ),
        address.toString().length > 30
            ? Text(
                '' + address.substring(0, 30) + "...",
                style: TextStyle(
                    fontSize: 18.0,
                    color: Color(0xFF006064),
                    fontWeight: FontWeight.w300),
              )
            : Text(
                address,
                style: TextStyle(
                    fontSize: 18.0,
                    color: Color(0xFF006064),
                    fontWeight: FontWeight.w300),
              ),

        //Rating

        SizedBox(
          height: 10,
        ),
        CountingStars(this.averageRating, this.role),

        //Bio
        Container(
          child: Padding(
            padding:
                const EdgeInsets.symmetric(vertical: 30.0, horizontal: 10.0),
            child: Column(
              children: <Widget>[
                Text(
                  "Bio:",
                  style: TextStyle(
                    color: Color(0xFF006064),
                    fontSize: 18.0,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                SizedBox(
                  height: 10.0,
                ),
                Text(
                  this.bio,
                  style: TextStyle(
                    fontSize: 16.0,
                    color: Color(0xFF006064),
                    fontWeight: FontWeight.w300,
                  ),
                ),
              ],
            ),
          ),
        ),

        //Redes sociais

        Text(
          'Redes Sociais:',
          style: TextStyle(
            fontSize: 16.0,
            color: Color(0xFF006064),
            fontWeight: FontWeight.bold,
          ),
        ),
        Padding(
          padding: EdgeInsets.symmetric(vertical: 10.0),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: <Widget>[
              //Facebook
              Container(
                child: FlatButton(
                  onPressed: () => print("Facebook"),
                  //perfil de Facebook do utilizador
                  child: Container(
                    height: 60.0,
                    width: 60.0,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      color: Colors.white,
                      boxShadow: [
                        BoxShadow(
                          color: Colors.black26,
                          offset: Offset(0, 2),
                          blurRadius: 6.0,
                        ),
                      ],
                      image: DecorationImage(
                        image: AssetImage('assets/images/facebook.jpg'),
                      ),
                    ),
                  ),
                ),
              ),

              //Linkedin
              Container(
                child: FlatButton(
                  onPressed: () => print('LinkedIn'),
                  //perfil de LinkedIn do utilizador
                  child: Container(
                    height: 60.0,
                    width: 60.0,
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      color: Colors.white,
                      boxShadow: [
                        BoxShadow(
                          color: Colors.black26,
                          offset: Offset(0, 2),
                          blurRadius: 6.0,
                        ),
                      ],
                      image: DecorationImage(
                        image: AssetImage('assets/images/linkedin.png'),
                      ),
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
      ]);
    }
  }
}
