import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/enums/roles.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/pages/employer_review_page.dart';
import 'package:housem8_flutter/pages/mate_home_page.dart';
import 'package:housem8_flutter/pages/recover_password_page_1.dart';
import 'package:housem8_flutter/pages/welcome_page.dart';
import 'package:housem8_flutter/services/login_service.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';

import 'employer_home_page.dart';

class LoginScreen extends StatefulWidget {
  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: <Widget>[
          Container(
            height: double.infinity,
            width: double.infinity,
            decoration: BoxDecoration(
              color: Color(0xFFE0E0E0),
            ),
          ),
          Container(
            height: double.infinity,
            child: SingleChildScrollView(
              physics: AlwaysScrollableScrollPhysics(),
              padding: EdgeInsets.symmetric(
                horizontal: 40.0,
                vertical: 30.0,
              ),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: <Widget>[
                  Container(
                    child: new Image.asset(
                      'assets/images/logo_housem8.png',
                      height: 200.0,
                    ),
                  ),

                  //Email
                  SizedBox(height: 25.0),
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: <Widget>[
                      Container(
                        alignment: Alignment.centerLeft,
                        decoration: BoxDecoration(
                          color: Colors.white,
                          borderRadius: BorderRadius.circular(10.0),
                          boxShadow: [
                            BoxShadow(
                              color: Colors.black12,
                              blurRadius: 6.0,
                              offset: Offset(0, 2),
                            ),
                          ],
                        ),
                        height: 60.0,
                        child: TextField(
                          controller: _emailController,
                          keyboardType: TextInputType.emailAddress,
                          style: TextStyle(color: Color(0xFF5B82AA)),
                          decoration: InputDecoration(
                            border: InputBorder.none,
                            contentPadding: EdgeInsets.only(top: 14.0),
                            prefixIcon: Icon(
                              Icons.email,
                              color: Color(0xFF5B82AA),
                            ),
                            hintText: 'Email',
                            hintStyle: TextStyle(color: Color(0xFF5B82AA)),
                          ),
                        ),
                      )
                    ],
                  ),

                  //Password
                  SizedBox(height: 20.0),
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: <Widget>[
                      Container(
                        alignment: Alignment.centerLeft,
                        decoration: BoxDecoration(
                          color: Colors.white,
                          borderRadius: BorderRadius.circular(10.0),
                          boxShadow: [
                            BoxShadow(
                              color: Colors.black12,
                              blurRadius: 6.0,
                              offset: Offset(0, 2),
                            ),
                          ],
                        ),
                        height: 60.0,
                        child: TextField(
                          controller: _passwordController,
                          obscureText: true,
                          style: TextStyle(color: Color(0xFF5B82AA)),
                          decoration: InputDecoration(
                            border: InputBorder.none,
                            contentPadding: EdgeInsets.only(top: 14.0),
                            prefixIcon: Icon(
                              Icons.lock,
                              color: Color(0xFF5B82AA),
                            ),
                            hintText: 'Password',
                            hintStyle: TextStyle(color: Color(0xFF5B82AA)),
                          ),
                        ),
                      )
                    ],
                  ),

                  //Esqueceu-se da password
                  Container(
                    alignment: Alignment.centerRight,
                    child: FlatButton(
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => ForgotPasswordScreen()),
                        );
                      },
                      padding: EdgeInsets.only(right: 0.0),
                      child: Text(
                        'Esqueceu-se da password?',
                        style: TextStyle(
                          color: Color(0xFF5B82AA),
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ),
                  ),

                  //Botão de Login
                  Container(
                    padding: EdgeInsets.symmetric(vertical: 20.0),
                    width: double.infinity,
                    child: RaisedButton(
                      elevation: 5.0,
                      onPressed: () async {
                        await actionLogin();
                      },

                      //editar para ir para página a seguir ao login
                      padding: EdgeInsets.all(15.0),
                      shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(30.0)),
                      color: Colors.white,
                      child: Text(
                        'LOGIN',
                        style: TextStyle(
                          color: Color(0xFF5B82AA),
                          letterSpacing: 1.5,
                          fontSize: 18.0,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ),
                  ),

                  //Login com Facebook ou Linkedin
                  Column(
                    children: <Widget>[
                      Text(
                        ' - OU - ',
                        style: TextStyle(
                          color: Color(0xFF5B82AA),
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                      SizedBox(height: 20.0),
                      Text(
                        'Faça login com',
                        style: TextStyle(
                          color: Color(0xFF5B82AA),
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ],
                  ),

                  Padding(
                    padding: EdgeInsets.symmetric(vertical: 20.0),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                      children: <Widget>[
                        //Facebook
                        Container(
                          child: FlatButton(
                            onPressed: () {
                              Navigator.push(
                                context,
                                MaterialPageRoute(
                                    builder: (context) => EmployerReviewPage()),
                              );
                            },
                            //editar para login com Facebook
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
                                  image:
                                      AssetImage('assets/images/facebook.jpg'),
                                ),
                              ),
                            ),
                          ),
                        ),

                        //Linkedin
                        Container(
                          child: FlatButton(
                            onPressed: () => print('Login com LinkedIn'),
                            //editar para login com linkedin
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
                                  image:
                                      AssetImage('assets/images/linkedin.png'),
                                ),
                              ),
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),

                  //Don't have an account?
                  Container(
                    child: FlatButton(
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => WelcomeScreen()),
                        );
                      },
                      //editar para ir para a página de registo
                      child: RichText(
                        text: TextSpan(
                          children: [
                            TextSpan(
                              text: 'Não tem uma conta? ',
                              style: TextStyle(
                                color: Color(0xFF5B82AA),
                                fontSize: 18.0,
                                fontWeight: FontWeight.w400,
                              ),
                            ),
                            TextSpan(
                              text: "Registe-se!",
                              style: TextStyle(
                                color: Color(0xFF5B82AA),
                                fontSize: 18.0,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  Future<void> actionLogin() async {
    if (await ConnectionHelper.checkConnection()) {
      var email = _emailController.text;
      var password = _passwordController.text;

      var statusCode = await LoginService().loginUser(email, password);

      if (statusCode == 200) {
        String role = await StorageHelper.readTokenRole();
        if (EnumToString.fromString(Roles.values, role) == Roles.M8) {
          Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => MateHomePage()),
          );
        } else if (EnumToString.fromString(Roles.values, role) ==
            Roles.EMPLOYER) {
          Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => EmployerHomePage()),
          );
        }
      } else {
        showDialog(
          context: context,
          builder: (context) => ErrorMessageDialog(
              title: "Dados Incorretos",
              text: "Os dados inseridos não se encontram corretos!"),
        );
      }
    } else {
      showDialog(
        context: context,
        builder: (context) => ErrorMessageDialog(
            title: "Sem conexão",
            text: "Dispositivo não consegue conectar ao servidor!"),
      );
    }
  }
}
