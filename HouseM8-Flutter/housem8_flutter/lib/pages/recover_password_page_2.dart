import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/recover_password.dart';
import 'package:housem8_flutter/pages/login_page.dart';
import 'package:housem8_flutter/services/recover_password_service.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';

class RecoverPasswordScreen extends StatefulWidget {
  @override
  _RecoverPasswordScreen createState() => _RecoverPasswordScreen();
}

class _RecoverPasswordScreen extends State<RecoverPasswordScreen> {
  String _email;
  String _password;
  String _confirmPassword;
  String _token;

  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();

  Widget emailWidget() {
    return TextFormField(
      decoration:
          InputDecoration(labelText: 'Email', helperText: 'Insira o seu email'),
      keyboardType: TextInputType.emailAddress,
      validator: (String value) {
        String pattern =
            r'^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$';
        RegExp regExp = new RegExp(pattern);
        if (value.length == 0) {
          return "Insira um email";
        } else if (!regExp.hasMatch(value)) {
          return "Email inválido";
        } else {
          return null;
        }
      },
      onSaved: (String value) {
        _email = value;
      },
    );
  }

  Widget passwordWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Password', helperText: 'Insira uma password'),
      obscureText: true,
      validator: (String value) {
        String pattern =
            r'^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#\$&*~]).{8,}$';
        RegExp regExp = new RegExp(pattern);
        if (value.length == 0) {
          return "Insira uma password";
        } else if (!regExp.hasMatch(value)) {
          return "Uma letra maiscula, um digito e um caracter especial";
        } else {
          return null;
        }
      },
      onSaved: (String value) {
        _password = value;
      },
    );
  }

  Widget confirmPasswordWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Confirme a password', helperText: 'Insira uma password'),
      obscureText: true,
      validator: (String value) {
        String pattern =
            r'^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#\$&*~]).{8,}$';
        RegExp regExp = new RegExp(pattern);
        if (value.length == 0) {
          return "Insira uma password";
        } else if (!regExp.hasMatch(value)) {
          return "Uma letra maiscula, um digito e um caracter especial";
        } else {
          return null;
        }
      },
      onSaved: (String value) {
        _confirmPassword = value;
      },
    );
  }

  Widget tokenWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Token', helperText: 'Insira o token recebido no e-mail'),
      validator: (String value) {
        if (value.length == 0) {
          return "Introduza o token";
        }
        return null;
      },
      onSaved: (String value) {
        _token = value;
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Pedido de alteração de pass")),
      body: SingleChildScrollView(
        child: Container(
          margin: EdgeInsets.all(24),
          child: Form(
            key: _formKey,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: <Widget>[
                Container(
                  padding: EdgeInsets.fromLTRB(16.0, 110.0, 0.0, 0.0),
                  child: Text(
                      'Foi enviado um e-mail com um token para verificação de identidade. \n'
                      'Para alterar a palavra-passe, preencha os seguintes campos',
                      style: TextStyle(
                          fontSize: 20.0,
                          fontWeight: FontWeight.bold,
                          color: Color(0xFF5B82AA))),
                ),
                emailWidget(),
                passwordWidget(),
                confirmPasswordWidget(),
                tokenWidget(),
                SizedBox(height: 30),
                RaisedButton(
                  padding: EdgeInsets.all(15.0),
                  shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(30.0)),
                  color: Colors.white,
                  child: Text(
                    'Seguinte',
                    style: TextStyle(color: Colors.blue, fontSize: 16),
                  ),
                  onPressed: () async {
                    if (!_formKey.currentState.validate()) {
                      return;
                    }
                    _formKey.currentState.save();
                    await actionChangePassword();

                    //Send to API
                  },
                )
              ],
            ),
          ),
        ),
      ),
    );
  }

  // passar os dados
  Future<void> actionChangePassword() async {
    if (await ConnectionHelper.checkConnection()) {
      var email = _email;
      var password = _password;
      var confirmPassword = _confirmPassword;
      var token = _token;

      RecoverPassword newPassword = new RecoverPassword(
          email: email,
          password: password,
          confirmPassword: confirmPassword,
          token: token);

      var statusCode =
          await RecoverPasswordService().changePassword(newPassword);

      if (statusCode == 200) {
        Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => LoginScreen()),
        );
        Fluttertoast.showToast(
            msg: "Agora pode fazer Login com os seus dados",
            toastLength: Toast.LENGTH_LONG,
            gravity: ToastGravity.BOTTOM,
            timeInSecForIosWeb: 5,
            backgroundColor: Colors.lightBlue,
            textColor: Colors.white,
            fontSize: 16.0);
      } else {
        showDialog(
          context: context,
          builder: (context) => ErrorMessageDialog(
              title: "Dado(s) invalido(s)",
              text: "O(s) dado(s) inserido(s) encontram-se invalido(s)!"),
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
