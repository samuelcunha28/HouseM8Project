import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/forgot_password.dart';
import 'package:housem8_flutter/pages/recover_password_page_2.dart';
import 'package:housem8_flutter/services/send_token_recover_password_service.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';

class ForgotPasswordScreen extends StatefulWidget {
  @override
  _ForgotPasswordScreen createState() => _ForgotPasswordScreen();
}

class _ForgotPasswordScreen extends State<ForgotPasswordScreen> {
  String _email;

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
                  padding: EdgeInsets.fromLTRB(16.0, 20.0, 16.0, 0.0),
                  child: new Image.network(
                      'https://onetwopixel.com/wp-content/uploads/2018/02/animat-lock-color.gif'),
                ),
                Container(
                  padding: EdgeInsets.fromLTRB(16.0, 5.0, 16.0, 0.0),
                  child: Text(
                      'Para alterar a palavra-passe, por favor insira um e-mail válido e registado na aplicação',
                      style: TextStyle(
                          fontSize: 15.0,
                          fontWeight: FontWeight.bold,
                          color: Color(0xFF5B82AA))),
                ),
                emailWidget(),
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
                    await actionSendToken();

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
  Future<void> actionSendToken() async {
    if (await ConnectionHelper.checkConnection()) {
      var email = _email;

      ForgotPassword password = new ForgotPassword(email: email);

      var statusCode = await SendTokenService().sendToken(password);

      if (statusCode == 200) {
        Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => RecoverPasswordScreen()),
        );
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
