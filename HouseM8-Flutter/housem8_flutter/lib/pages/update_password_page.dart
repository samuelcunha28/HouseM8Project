import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/password_update.dart';
import 'package:housem8_flutter/services/update_password_service.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';

import 'login_page.dart';

class UpdatePasswordPage extends StatefulWidget {
  final String role;

  UpdatePasswordPage({
    Key key,
    this.role,
  }) : super(key: key);

  @override
  _UpdatePasswordPageState createState() => _UpdatePasswordPageState(this.role);
}

class _UpdatePasswordPageState extends State<UpdatePasswordPage> {
  final String role;

  _UpdatePasswordPageState(
    this.role,
  );

  final TextEditingController _oldPasswordController = TextEditingController();
  final TextEditingController _newPasswordController = TextEditingController();
  FocusNode myFocusNode = FocusNode();

  @override
  Widget build(BuildContext context) {
    if (this.role == "MATE") {
      return Scaffold(
        appBar: AppBar(
          centerTitle: true,
          title: Text("Editar Palavra Passe"),
          backgroundColor: Color(0xFF39A3ED),
          actions: <Widget>[
            IconButton(
              icon: Icon(Icons.save),
              tooltip: 'Guardar alterações',
              onPressed: () async {
                await actionUpdatePassword();
              },
            ),
          ],
        ),
        body: SingleChildScrollView(
          child: Container(
            child: Padding(
              padding: EdgeInsets.symmetric(vertical: 10.0, horizontal: 30.0),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  SizedBox(
                    height: 170.0,
                  ),
                  Text(
                    "Introduza a palavra passe atual",
                    style: TextStyle(fontSize: 20, fontWeight: FontWeight.w600),
                  ),
                  TextFormField(
                    controller: _oldPasswordController,
                    decoration: InputDecoration(
                        labelText: 'Palavra passe:',
                        labelStyle: TextStyle(fontSize: 20)),
                    keyboardType: TextInputType.visiblePassword,
                    obscureText: true,
                    style: TextStyle(fontSize: 20),
                  ),
                  SizedBox(
                    height: 50.0,
                  ),
                  Text(
                    "Introduza a nova palavra passe",
                    style: TextStyle(fontSize: 20, fontWeight: FontWeight.w600),
                  ),
                  TextFormField(
                    controller: _newPasswordController,
                    decoration: InputDecoration(
                        labelText: 'Palavra passe:',
                        labelStyle: TextStyle(fontSize: 20)),
                    keyboardType: TextInputType.visiblePassword,
                    obscureText: true,
                    style: TextStyle(fontSize: 20),
                  ),
                ],
              ),
            ),
          ),
        ),
      );
    } else {
      return Scaffold(
        appBar: AppBar(
          centerTitle: true,
          title: Text("Editar Palavra Passe"),
          backgroundColor: Color(0xFF93C901),
          actions: <Widget>[
            IconButton(
              icon: Icon(Icons.save),
              tooltip: 'Guardar alterações',
              onPressed: () async {
                await actionUpdatePassword();
              },
            ),
          ],
        ),
        body: SingleChildScrollView(
          child: Container(
            child: Padding(
              padding: EdgeInsets.symmetric(vertical: 10.0, horizontal: 30.0),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  SizedBox(
                    height: 170.0,
                  ),
                  Text(
                    "Introduza a palavra passe atual",
                    style: TextStyle(fontSize: 20, fontWeight: FontWeight.w600),
                  ),
                  TextFormField(
                    controller: _oldPasswordController,
                    decoration: InputDecoration(
                        labelText: 'Palavra passe:',
                        labelStyle: TextStyle(
                          fontSize: 20,
                          color: myFocusNode.hasFocus
                              ? Color(0xFF006064)
                              : Colors.grey,
                        ),
                        focusedBorder: new UnderlineInputBorder(
                            borderSide:
                                new BorderSide(color: Color(0xFF006064)))),
                    keyboardType: TextInputType.visiblePassword,
                    obscureText: true,
                    style: TextStyle(fontSize: 20),
                  ),
                  SizedBox(
                    height: 50.0,
                  ),
                  Text(
                    "Introduza a nova palavra passe",
                    style: TextStyle(fontSize: 20, fontWeight: FontWeight.w600),
                  ),
                  TextFormField(
                    controller: _newPasswordController,
                    decoration: InputDecoration(
                        labelText: 'Palavra passe:',
                        labelStyle: TextStyle(
                          fontSize: 20,
                          color: myFocusNode.hasFocus
                              ? Color(0xFF006064)
                              : Colors.grey,
                        ),
                        focusedBorder: new UnderlineInputBorder(
                            borderSide:
                                new BorderSide(color: Color(0xFF006064)))),
                    keyboardType: TextInputType.visiblePassword,
                    obscureText: true,
                    style: TextStyle(fontSize: 20),
                  ),
                ],
              ),
            ),
          ),
        ),
      );
    }
  }

  Future<void> actionUpdatePassword() async {
    if (await ConnectionHelper.checkConnection()) {
      var oldPassword = _oldPasswordController.text;
      var newPassword = _newPasswordController.text;

      PasswordUpdate update =
          new PasswordUpdate(password: newPassword, oldPassword: oldPassword);

      var statusCode = await UpdatePasswordService().updatePassword(update);

      if (statusCode == 200) {
        StorageHelper.deleteAllTokenData();
        Navigator.pushReplacement(
            context, MaterialPageRoute(builder: (context) => LoginScreen()));
      } else {
        showDialog(
          context: context,
          builder: (context) => ErrorMessageDialog(
              title: "Sem Autorização",
              text: "Não tem autorização para efetuar esta operação!"),
        );
      }
    } else {
      showDialog(
        context: context,
        builder: (context) => ErrorMessageDialog(
            title: "Sem conexão",
            text: "Dispositivo não se consegue conectar ao servidor!"),
      );
    }
  }
}
