import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/enums/categories.dart';
import 'package:housem8_flutter/enums/payment.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/address.dart';
import 'package:housem8_flutter/models/job_post_publication.dart';
import 'package:housem8_flutter/view_models/employer_post_list_view.dart';
import 'package:housem8_flutter/view_models/employer_post_view.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:multi_select_flutter/multi_select_flutter.dart';
import 'package:provider/provider.dart';
import 'package:recase/recase.dart';

class UpdateJobPostPage extends StatefulWidget {
  final EmployerPostViewModel post;

  const UpdateJobPostPage({Key key, this.post}) : super(key: key);

  @override
  _UpdateJobPostPageState createState() => _UpdateJobPostPageState();
}

class _UpdateJobPostPageState extends State<UpdateJobPostPage> {
  EmployerPostListViewModel vm;
  JobPostPublication jobPost = JobPostPublication(address: Address());
  bool isDeviceConnected = false;
  var internetConnection;
  Color color = Color(0xFF93C901);
  Categories categoryValue = null;
  bool isTradable = false;

  @override
  void initState() {
    super.initState();
    vm = Provider.of<EmployerPostListViewModel>(context, listen: false);

    ConnectionHelper.checkConnection().then((value) {
      isDeviceConnected = value;
      setState(() {});
    });

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
            title: Text("Editar Publicação"),
            backgroundColor: Color(0xFF93C901),
            actions: <Widget>[
              IconButton(
                icon: Icon(Icons.save),
                tooltip: 'Guardar',
                onPressed: () {
                  updateDataFromService(jobPost);
                  Navigator.of(context).pop();
                },
              ),
            ],
          ),
          body: Container(
              child: SingleChildScrollView(
                  child: Column(children: <Widget>[
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
                            onPressed: () => print("Editar foto de perfil"),
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
                    TextFormField(
                      decoration: InputDecoration(
                        labelText: 'Título',
                        labelStyle: TextStyle(color: Colors.black),
                        focusedBorder: UnderlineInputBorder(
                            borderSide: BorderSide(color: color)),
                      ),
                      cursorColor: color,
                      style: TextStyle(decorationColor: color),
                      initialValue: widget.post.title,
                      onChanged: (title) {
                        jobPost.title = title;
                      },
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
                      cursorColor: color,
                      style: TextStyle(decorationColor: color),
                      initialValue: widget.post.description,
                      onChanged: (description) {
                        jobPost.description = description;
                      },
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
                      keyboardType: TextInputType.number,
                      initialValue: widget.post.initialPrice.toString(),
                      onChanged: (initialPrice) {
                        jobPost.initialPrice = double.parse(initialPrice);
                      },
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
                      style: TextStyle(decorationColor: color),
                      initialValue: widget.post.address.split(",").first,
                      onChanged: (street) {
                        jobPost.address.street = street;
                      },
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
                      style: TextStyle(decorationColor: color),
                      keyboardType: TextInputType.number,
                      initialValue: RegExp(r"[0-9]\S+")
                          .allMatches(widget.post.address)
                          .elementAt(0)
                          .group(0),
                      onChanged: (streetNumber) {
                        jobPost.address.streetNumber = int.parse(streetNumber);
                      },
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
                      style: TextStyle(decorationColor: color),
                      initialValue: RegExp(r"[0-9]\S+")
                          .allMatches(widget.post.address)
                          .elementAt(1)
                          .group(0),
                      onChanged: (postalCode) {
                        jobPost.address.postalCode = postalCode;
                      },
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
                      style: TextStyle(decorationColor: color),
                      initialValue: RegExp(r"[0-9] ([a-zA-Z]*),")
                          .allMatches(widget.post.address)
                          .elementAt(0)
                          .group(1),
                      onChanged: (district) {
                        jobPost.address.district = district;
                      },
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
                      style: TextStyle(decorationColor: color),
                      initialValue: widget.post.address.split(",").last.trim(),
                      onChanged: (country) {
                        jobPost.address.country = country;
                      },
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
                          child: Text(
                              new ReCase(EnumToString.convertToString(category))
                                  .titleCase),
                        );
                      }).toList(),
                      onChanged: (Categories newValue) {
                        jobPost.category = newValue;
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
                        "Negociável?",
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
                        MultiSelectItem<Payment>(Payment.MONEY, "Dinheiro"),
                        MultiSelectItem<Payment>(Payment.PAYPAL, "Paypal")
                      ],
                      onConfirm: (values) {
                        jobPost.paymentMethod = values;
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
          ]))));
    } else {
      return Scaffold(
          appBar: AppBar(
            centerTitle: true,
            title: Text("Editar Publicação"),
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

  void updateDataFromService(JobPostPublication jobPostPublication) {
    if (jobPostPublication.title == null) {
      jobPostPublication.title = widget.post.title;
    }

    if (jobPostPublication.description == null) {
      jobPostPublication.description = widget.post.description;
    }

    if (jobPostPublication.initialPrice == null) {
      jobPostPublication.initialPrice = widget.post.initialPrice;
    }

    if (jobPostPublication.address.street == null) {
      jobPostPublication.address.street = widget.post.address.split(",").first;
    }

    if (jobPostPublication.address.streetNumber == null) {
      jobPostPublication.address.streetNumber = int.parse(RegExp(r"[0-9]\S+")
          .allMatches(widget.post.address)
          .elementAt(0)
          .group(0));
    }

    if (jobPostPublication.address.postalCode == null) {
      jobPostPublication.address.postalCode = RegExp(r"[0-9]\S+")
          .allMatches(widget.post.address)
          .elementAt(1)
          .group(0);
    }

    if (jobPostPublication.address.district == null) {
      jobPostPublication.address.district = RegExp(r"[0-9] ([a-zA-Z]*),")
          .allMatches(widget.post.address)
          .elementAt(0)
          .group(1);
    }

    if (jobPostPublication.address.country == null) {
      jobPostPublication.address.country =
          widget.post.address.split(",").last.trim();
    }

    if (jobPostPublication.category == null) {
      jobPostPublication.category = Categories.GARDENING;
    }

    if (jobPostPublication.tradable == null) {
      jobPostPublication.tradable = false;
    }

    if (jobPostPublication.paymentMethod == null) {
      List<Payment> payment = List<Payment>();
      payment.add(Payment.MONEY);
      jobPostPublication.paymentMethod = payment;
    }

    vm.uploadEmployerPost(jobPostPublication, widget.post.id);
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
