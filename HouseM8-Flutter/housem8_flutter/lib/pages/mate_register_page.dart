import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:housem8_flutter/enums/categories.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/address.dart';
import 'package:housem8_flutter/models/mate_register.dart';
import 'package:housem8_flutter/pages/login_page.dart';
import 'package:housem8_flutter/services/register_mate_service.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';
import 'package:multi_select_flutter/multi_select_flutter.dart';

class MateRegisterPage extends StatefulWidget {
  @override
  _MateRegisterPage createState() => _MateRegisterPage();
}

class _MateRegisterPage extends State<MateRegisterPage> {
  String _firstName;
  String _lastName;
  String _userName;
  String _password;
  String _email;
  String _description;
  String _street;
  String _streetNumber;
  String _postalCode;
  String _disctrict;
  String _country;
  List<Categories> _category;
  String _range;

  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();

  static List<String> _categories = [
    Categories.CLEANING.toString().split('.').last,
    Categories.TRANSPORTATION.toString().split('.').last,
    Categories.INTERIOR_DESIGN.toString().split('.').last,
    Categories.FURNITURE_ASSEMBLE.toString().split('.').last,
    Categories.ELECTRICITY.toString().split('.').last,
    Categories.GARDENING.toString().split('.').last,
    Categories.PLUMBING.toString().split('.').last,
  ];
  final _items = _categories
      .map((categories) =>
          MultiSelectItem<String>(categories, categories.toString()))
      .toList();
  final _multiSelectKey = GlobalKey<FormFieldState>();

  Widget firstNameWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Primeiro nome', helperText: 'Insira o seu primeiro nome'),
      maxLength: 15,
      validator: (String value) {
        String patttern = r'(^[a-zA-Z ]*$)';
        RegExp regExp = new RegExp(patttern);
        if (value.length == 0) {
          return "Introduza um nome";
        } else if (!regExp.hasMatch(value)) {
          return "O nome deve conter caracteres de a-z ou A-Z";
        }
        return null;
      },
      onSaved: (String value) {
        _firstName = value;
      },
    );
  }

  Widget lastNameWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Ultimo nome', helperText: 'Insira o seu ultimo nome'),
      maxLength: 15,
      validator: (String value) {
        String patttern = r'(^[a-zA-Z ]*$)';
        RegExp regExp = new RegExp(patttern);
        if (value.length == 0) {
          return "Introduza um nome";
        } else if (!regExp.hasMatch(value)) {
          return "O nome deve conter caracteres de a-z ou A-Z";
        }
        return null;
      },
      onSaved: (String value) {
        _lastName = value;
      },
    );
  }

  Widget usernameWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Username', helperText: 'Insira o seu username'),
      maxLength: 15,
      validator: (String value) {
        String patttern = r'(^[a-zA-Z ]*$)';
        RegExp regExp = new RegExp(patttern);
        if (value.length == 0) {
          return "Introduza um username";
        } else if (value.length < 5) {
          return "O username deve ter no mínimo 6 caracteres";
        }
        return null;
      },
      onSaved: (String value) {
        _userName = value;
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
          return "Uma letra maiúscula, um digito e um caracter especial";
        } else {
          return null;
        }
      },
      onSaved: (String value) {
        _password = value;
      },
    );
  }

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

  Widget streetWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Insira a rua', helperText: 'Insira a sua rua'),
      keyboardType: TextInputType.streetAddress,
      maxLength: 30,
      validator: (String value) {
        String patttern = r'(^[a-zA-Z ]*$)';
        RegExp regExp = new RegExp(patttern);
        if (value.length == 0) {
          return "Introduza uma rua";
        } else if (value.length < 5) {
          return "A rua deve ter no mínimo 6 caracteres";
        }
        return null;
      },
      onSaved: (String street) {
        _street = street;
      },
    );
  }

  Widget streetNumberWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Número de rua', helperText: 'Insira o número de rua'),
      keyboardType: TextInputType.number,
      maxLength: 20,
      validator: (String value) {
        String patttern = r'(^[a-zA-Z ]*$)';
        RegExp regExp = new RegExp(patttern);
        if (value.length == 0) {
          return "Introduza o número da rua";
        }
        return null;
      },
      onSaved: (String streetNumber) {
        _streetNumber = streetNumber;
      },
    );
  }

  Widget postalCodeWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Código Postal', helperText: 'Insira o seu código postal'),
      maxLength: 10,
      validator: (String value) {
        String patttern = r'(^[a-zA-Z ]*$)';
        RegExp regExp = new RegExp(patttern);
        if (value.length == 0) {
          return "Introduza o seu código postal";
        } else if (value.length < 3) {
          return "O código postal deve ter no minimo 4 caracteres";
        }
        return null;
      },
      onSaved: (String postalCode) {
        _postalCode = postalCode;
      },
    );
  }

  Widget districtWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Distrito', helperText: 'Insira o seu distrito'),
      maxLength: 15,
      validator: (String value) {
        String patttern = r'(^[a-zA-Z ]*$)';
        RegExp regExp = new RegExp(patttern);
        if (value.length == 0) {
          return "Introduza uma morada";
        } else if (value.length < 5) {
          return "O distrito deve ter no mínimo 6 caracteres";
        }
        return null;
      },
      onSaved: (String district) {
        _disctrict = district;
      },
    );
  }

  Widget countryWidget() {
    return TextFormField(
      decoration:
          InputDecoration(labelText: 'País', helperText: 'Insira o seu país'),
      maxLength: 15,
      validator: (String value) {
        String patttern = r'(^[a-zA-Z ]*$)';
        RegExp regExp = new RegExp(patttern);
        if (value.length == 0) {
          return "Introduza um país";
        } else if (value.length < 3) {
          return "O país deve ter no mínimo 3 caracteres";
        }
        return null;
      },
      onSaved: (String country) {
        _country = country;
      },
    );
  }

  Widget descriptionWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Descrição', helperText: 'Fale um pouco sobre si'),
      maxLength: 50,
      validator: (String value) {
        String patttern = r'(^[a-zA-Z ]*$)';
        RegExp regExp = new RegExp(patttern);
        if (value.length == 0) {
          return "Introduza uma descrição";
        } else if (value.length < 15) {
          return "A descrição deve ter no mínimo 15 caracteres";
        }
        return null;
      },
      onSaved: (String value) {
        _description = value;
      },
    );
  }

  Widget categoriesWidget() {
    return SingleChildScrollView(
      child: Container(
        alignment: Alignment.center,
        padding: EdgeInsets.all(20),
        child: Column(
          children: <Widget>[
            SizedBox(height: 40),
            MultiSelectDialogField(
              items: _items,
              title: Text("Trabalhos"),
              selectedColor: Colors.blue,
              decoration: BoxDecoration(
                color: Colors.blue.withOpacity(0.1),
                borderRadius: BorderRadius.all(Radius.circular(40)),
                border: Border.all(
                  color: Colors.blue,
                  width: 2,
                ),
              ),
              buttonIcon: Icon(
                Icons.work,
                color: Colors.blue,
              ),
              buttonText: Text(
                "Escolha 1 ou mais trabalhos",
                style: TextStyle(
                  color: Colors.blue[800],
                  fontSize: 16,
                ),
              ),
              onConfirm: (results) {
                _categories = results;
              },
              onSaved: (values) {
                _categories = values;
                _category = _categories
                    .map((category) =>
                        EnumToString.fromString(Categories.values, category))
                    .toList();
              },
            ),
          ],
        ),
      ),
    );
  }

  Widget rangeWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Alcance',
          helperText: 'Defina uma media de alcance (em km)'),
      keyboardType: TextInputType.number,
      maxLength: 3,
      validator: (String value) {
        if (value.length == 0) {
          return "Introduza um alcance";
        }
        return null;
      },
      onSaved: (String value) {
        _range = value;
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Registo de mate")),
      body: SingleChildScrollView(
        child: Container(
          margin: EdgeInsets.all(24),
          child: Form(
            key: _formKey,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: <Widget>[
                firstNameWidget(),
                lastNameWidget(),
                usernameWidget(),
                emailWidget(),
                passwordWidget(),
                streetWidget(),
                streetNumberWidget(),
                postalCodeWidget(),
                districtWidget(),
                countryWidget(),
                descriptionWidget(),
                SizedBox(height: 5.0),
                categoriesWidget(),
                SizedBox(height: 10.0),
                rangeWidget(),
                SizedBox(height: 30),
                RaisedButton(
                  padding: EdgeInsets.all(15.0),
                  shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(30.0)),
                  color: Colors.white,
                  child: Text(
                    'Submit',
                    style: TextStyle(color: Colors.blue, fontSize: 16),
                  ),
                  onPressed: () async {
                    if (!_formKey.currentState.validate()) {
                      return;
                    }
                    _formKey.currentState.save();
                    await actionRegisterMate();

                    Fluttertoast.showToast(
                        msg: "Agora pode fazer Login com os seus dados",
                        toastLength: Toast.LENGTH_LONG,
                        gravity: ToastGravity.BOTTOM,
                        timeInSecForIosWeb: 5,
                        backgroundColor: Colors.lightBlue,
                        textColor: Colors.white,
                        fontSize: 16.0);

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
  Future<void> actionRegisterMate() async {
    if (await ConnectionHelper.checkConnection()) {
      var firstName = _firstName;
      var lastName = _lastName;
      var userName = _userName;
      var email = _email;
      var password = _password;
      var street = _street;
      var streetNumber = _streetNumber;
      var postalCode = _postalCode;
      var district = _disctrict;
      var country = _country;
      var description = _description;
      var category = _category;
      var range = _range;

      Address address = new Address(
          street: street,
          streetNumber: int.parse(streetNumber.toString()),
          postalCode: postalCode,
          district: district,
          country: country);

      MateRegister register = new MateRegister(
          firstName: firstName,
          lastName: lastName,
          userName: userName,
          email: email,
          password: password,
          description: description,
          address: address,
          category: category,
          range: int.parse(range.toString()));

      var statusCode = await RegisterMateService().createMateRegister(register);

      if (statusCode == 200) {
        Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => LoginScreen()),
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
