import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/enums/categories.dart';
import 'package:housem8_flutter/enums/payment.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/address.dart';
import 'package:housem8_flutter/models/job_post_publication.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:multi_select_flutter/multi_select_flutter.dart';
import 'package:recase/recase.dart';

class CreateJobPostPage extends StatefulWidget {
  @override
  _CreateJobPostPageState createState() => _CreateJobPostPageState();
}

class _CreateJobPostPageState extends State<CreateJobPostPage> {
  final TextEditingController _titleController = TextEditingController();
  final TextEditingController _descriptionController = TextEditingController();
  final TextEditingController _priceController = TextEditingController();
  final TextEditingController _streetController = TextEditingController();
  final TextEditingController _streetNumberController = TextEditingController();
  final TextEditingController _postalCodeController = TextEditingController();
  final TextEditingController _districtController = TextEditingController();
  final TextEditingController _countryController = TextEditingController();

  JobPostPublication post = JobPostPublication(address: Address());
  bool isDeviceConnected = true;
  var internetConnection;
  Color color = Color(0xFF93C901);
  Categories categoryValue = null;
  bool isTradable = false;

  @override
  void initState() {
    super.initState();

    //Verificar Conectividade
    ConnectionHelper.checkConnection().then((value) {
      isDeviceConnected = value;
      setState(() {});
    });

    //Ativar listener para caso a conectividade mude
    internetConnection = Connectivity()
        .onConnectivityChanged
        .listen((ConnectivityResult result) async {
      if (result != ConnectivityResult.none) {
        isDeviceConnected = await DataConnectionChecker().hasConnection;
        setState(() {});
      } else {
        isDeviceConnected = false;
        setState(() {});
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    if (isDeviceConnected) {
      return Scaffold(
          appBar: AppBar(
            centerTitle: true,
            title: Text("Criar Publicação"),
            backgroundColor: Color(0xFF93C901),
            actions: <Widget>[
              IconButton(
                icon: Icon(Icons.save),
                tooltip: 'Guardar',
                onPressed: () {
                  save();
                  Navigator.pop(context, post);
                },
              ),
            ],
          ),
          body: Container(
            child: SingleChildScrollView(
              child: Column(
                children: <Widget>[
                  Container(
                    decoration: BoxDecoration(
                      image: DecorationImage(
                        image: NetworkImage(
                            "https://www.tibs.org.tw/images/default.jpg"),
                        fit: BoxFit.fitWidth,
                      ),
                      boxShadow: [
                        BoxShadow(
                          color: Colors.black12,
                          blurRadius: 6.0,
                          offset: Offset(0, 2),
                        ),
                      ],
                    ),
                    child: Container(
                      width: double.infinity,
                      height: 150,
                      child: Container(
                        alignment: Alignment(0.0, 10.0),
                        child: CircleAvatar(
                          backgroundColor: color,
                          radius: 70.0,
                          child: CircleAvatar(
                            child: Align(
                              alignment: Alignment.bottomRight,
                              child: CircleAvatar(
                                backgroundColor: color,
                                radius: 20.0,
                                child: IconButton(
                                  icon: Icon(Icons.add_a_photo,
                                      size: 25.0, color: Colors.white),
                                  tooltip: "Editar foto de perfil",
                                  onPressed: () =>
                                      print("Editar foto de perfil"),
                                ),
                              ),
                            ),
                            backgroundImage: NetworkImage(
                                "https://www.tibs.org.tw/images/default.jpg"),
                            radius: 68.0,
                          ),
                        ),
                      ),
                    ),
                  ),
                  Container(
                    child: Padding(
                      padding: EdgeInsets.symmetric(horizontal: 22.0),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: <Widget>[
                          SizedBox(
                            height: 70.0,
                          ),
                          Text(
                            "Preencha os dados abaixo: ",
                            style: TextStyle(
                                fontSize: 18, fontWeight: FontWeight.w600),
                          ),
                          TextFormField(
                            decoration: InputDecoration(
                              labelText: 'Título',
                              labelStyle: TextStyle(color: Colors.black),
                              focusedBorder: UnderlineInputBorder(
                                  borderSide: BorderSide(color: color)),
                            ),
                            controller: _titleController,
                            cursorColor: color,
                            style: TextStyle(decorationColor: color),
                          ),
                          SizedBox(
                            height: 10.0,
                          ),
                          TextFormField(
                            decoration: InputDecoration(
                              labelText: 'Descrição',
                              labelStyle: TextStyle(color: Colors.black),
                              focusedBorder: UnderlineInputBorder(
                                  borderSide: BorderSide(color: color)),
                            ),
                            controller: _descriptionController,
                            cursorColor: color,
                            style: TextStyle(decorationColor: color),
                          ),
                          SizedBox(
                            height: 10.0,
                          ),
                          TextFormField(
                            decoration: InputDecoration(
                              labelText: 'Preço Inicial',
                              labelStyle: TextStyle(color: Colors.black),
                              focusedBorder: UnderlineInputBorder(
                                  borderSide: BorderSide(color: color)),
                            ),
                            controller: _priceController,
                            keyboardType: TextInputType.number,
                          ),
                          SizedBox(
                            height: 10.0,
                          ),
                          TextFormField(
                            decoration: InputDecoration(
                              labelText: 'Rua',
                              labelStyle: TextStyle(color: Colors.black),
                              focusedBorder: UnderlineInputBorder(
                                  borderSide: BorderSide(color: color)),
                            ),
                            cursorColor: color,
                            controller: _streetController,
                            style: TextStyle(decorationColor: color),
                          ),
                          SizedBox(
                            height: 10.0,
                          ),
                          TextFormField(
                            decoration: InputDecoration(
                              labelText: 'Número de Porta',
                              labelStyle: TextStyle(color: Colors.black),
                              focusedBorder: UnderlineInputBorder(
                                  borderSide: BorderSide(color: color)),
                            ),
                            cursorColor: color,
                            controller: _streetNumberController,
                            style: TextStyle(decorationColor: color),
                            keyboardType: TextInputType.number,
                          ),
                          SizedBox(
                            height: 10.0,
                          ),
                          TextFormField(
                            decoration: InputDecoration(
                              labelText: 'Código postal',
                              labelStyle: TextStyle(color: Colors.black),
                              focusedBorder: UnderlineInputBorder(
                                  borderSide: BorderSide(color: color)),
                            ),
                            cursorColor: color,
                            controller: _postalCodeController,
                            style: TextStyle(decorationColor: color),
                          ),
                          SizedBox(
                            height: 10.0,
                          ),
                          TextFormField(
                            decoration: InputDecoration(
                              labelText: 'Cidade',
                              labelStyle: TextStyle(color: Colors.black),
                              focusedBorder: UnderlineInputBorder(
                                  borderSide: BorderSide(color: color)),
                            ),
                            cursorColor: color,
                            controller: _districtController,
                            style: TextStyle(decorationColor: color),
                          ),
                          SizedBox(
                            height: 10.0,
                          ),
                          TextFormField(
                            decoration: InputDecoration(
                              labelText: 'País',
                              labelStyle: TextStyle(color: Colors.black),
                              focusedBorder: UnderlineInputBorder(
                                  borderSide: BorderSide(color: color)),
                            ),
                            cursorColor: color,
                            controller: _countryController,
                            style: TextStyle(decorationColor: color),
                          ),
                          SizedBox(
                            height: 10.0,
                          ),
                          DropdownButtonFormField<Categories>(
                            decoration: InputDecoration(
                              labelText: 'Categoria',
                              labelStyle: TextStyle(color: Colors.black),
                            ),
                            value: categoryValue,
                            icon: Icon(Icons.arrow_drop_down),
                            iconSize: 24,
                            isExpanded: true,
                            elevation: 16,
                            items: Categories.values.map((Categories category) {
                              return DropdownMenuItem<Categories>(
                                value: category,
                                child: Text(new ReCase(
                                        EnumToString.convertToString(category))
                                    .titleCase),
                              );
                            }).toList(),
                            onChanged: (Categories newValue) {
                              post.category = newValue;
                              setState(() {
                                categoryValue = newValue;
                              });
                            },
                          ),
                          SizedBox(
                            height: 20.0,
                          ),
                          CheckboxListTile(
                            title: Text(
                              "Negociável",
                              textWidthBasis: TextWidthBasis.longestLine,
                            ),
                            activeColor: color,
                            contentPadding: EdgeInsets.zero,
                            value: isTradable,
                            onChanged: (newValue) {
                              setState(() {
                                isTradable = newValue;
                              });
                            }, //  <-- leading Checkbox
                          ),
                          SizedBox(
                            height: 20.0,
                          ),
                          Text(
                            "Tipos de Pagamentos: ",
                            style: TextStyle(fontSize: 16),
                          ),
                          MultiSelectBottomSheetField<Payment>(
                            title: Text(
                              "Tipos de Pagamentos",
                            ),
                            selectedColor: color,
                            buttonText: Text("Escolher",
                                style: TextStyle(
                                    fontSize: 14, fontWeight: FontWeight.w400)),
                            confirmText: Text("Confirmar"),
                            cancelText: Text("Cancelar"),
                            items: [
                              MultiSelectItem<Payment>(
                                  Payment.MONEY, "Dinheiro"),
                              MultiSelectItem<Payment>(Payment.PAYPAL, "Paypal")
                            ],
                            onConfirm: (values) {
                              post.paymentMethod = values;
                              setState(() {});
                            },
                            chipDisplay: MultiSelectChipDisplay(
                              chipColor: color,
                              textStyle: TextStyle(color: Colors.white),
                            ),
                          ),
                          SizedBox(
                            height: 25.0,
                          ),
                        ],
                      ),
                    ),
                  )
                ],
              ),
            ),
          ));
    } else {
      return Scaffold(
          appBar: AppBar(
            centerTitle: true,
            title: Text("Criar Publicação"),
            backgroundColor: Color(0xFF93C901),
            actions: <Widget>[
              IconButton(
                icon: Icon(Icons.save),
                tooltip: 'Guardar',
                onPressed: () {
                  showDialog(
                    context: context,
                    builder: (context) => ErrorMessageDialog(
                        title: "Dispositivo Offline!",
                        text: "O dispositivo não está conectado á internet!"),
                  );
                },
              ),
            ],
          ),
          body: OfflineMessage());
    }
  }

  void save() {
    if (_titleController != null) {
      post.title = _titleController.text;
    } else {
      post.title = "Sem Título!";
    }

    if (_descriptionController != null) {
      post.description = _descriptionController.text;
    } else {
      post.description = "Sem descrição!";
    }

    if (_priceController != null) {
      post.initialPrice = double.parse(_priceController.text);
    } else {
      post.initialPrice = 20.0;
    }

    if (_streetController != null) {
      post.address.street = _streetController.text;
    } else {
      post.address.street = "Rua Eng. Luís Carneiro Leão";
      post.address.streetNumber = 54;
      post.address.postalCode = "4590-244";
      post.address.district = "Porto";
      post.address.country = "Portugal";
    }

    if (_streetNumberController != null) {
      post.address.streetNumber = int.parse(_streetNumberController.text);
    } else {
      post.address.streetNumber = null;
    }

    if (_postalCodeController != null) {
      post.address.postalCode = _postalCodeController.text;
    } else {
      post.address.postalCode = "";
    }

    if (_districtController != null) {
      post.address.district = _districtController.text;
    } else {
      post.address.district = "";
    }

    if (_countryController != null) {
      post.address.country = _countryController.text;
    } else {
      post.address.country = "";
    }

    if (post.category == null) {
      post.category = Categories.GARDENING;
    }

    post.tradable = false;

    if (post.paymentMethod == null) {
      List<Payment> payments = List<Payment>();
      payments.add(Payment.MONEY);
      post.paymentMethod = payments;
    }
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
