import 'package:flutter/material.dart';

class UpdateUsers extends StatefulWidget {
  final String role;
  final String firstName;
  final String lastName;
  final String address;
  final String bio;
  final String username;
  final ValueChanged<String> firstNameUpdated;
  final ValueChanged<String> lastNameUpdated;
  final ValueChanged<String> usernameUpdated;
  final ValueChanged<String> streetUpdated;
  final ValueChanged<int> streetNumberUpdated;
  final ValueChanged<String> postalCodeUpdated;
  final ValueChanged<String> districtUpdated;
  final ValueChanged<String> countryUpdated;
  final ValueChanged<String> bioUpdated;
  final ValueChanged<int> rangeUpdated;

  UpdateUsers({
    Key key,
    this.role,
    this.firstName,
    this.lastName,
    this.username,
    this.address,
    this.bio,
    this.firstNameUpdated,
    this.lastNameUpdated,
    this.usernameUpdated,
    this.streetUpdated,
    this.streetNumberUpdated,
    this.postalCodeUpdated,
    this.districtUpdated,
    this.countryUpdated,
    this.bioUpdated,
    this.rangeUpdated,
  }) : super(key: key);

  @override
  _UpdateUsersState createState() => _UpdateUsersState(this.role,
      this.firstName, this.lastName, this.username, this.address, this.bio);
}

class _UpdateUsersState extends State<UpdateUsers> {
  final String role;
  String firstName;
  String lastName;
  String address;
  String bio;
  String username;

  _UpdateUsersState(
    this.role,
    this.firstName,
    this.lastName,
    this.username,
    this.address,
    this.bio,
  );

  @override
  Widget build(BuildContext context) {
    if (this.role == "MATE") {
      return SingleChildScrollView(
        child: Column(
          children: <Widget>[
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
              child: Container(
                width: double.infinity,
                height: 150,
                child: Container(
                  alignment: Alignment(0.0, 15.0),
                  child: CircleAvatar(
                    radius: 70.0,
                    child: CircleAvatar(
                      child: Align(
                        alignment: Alignment.bottomRight,
                        child: CircleAvatar(
                          backgroundColor: Color(0xFF1565C0),
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
                          "https://icon-library.com/images/facebook-user-icon/facebook-user-icon-4.jpg"),
                      radius: 68.0,
                    ),
                  ),
                ),
              ),
            ),
            Container(
              child: Padding(
                padding: EdgeInsets.symmetric(vertical: 10.0, horizontal: 10.0),
                child: Column(
                  children: <Widget>[
                    SizedBox(
                      height: 85.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                        labelText: 'Primeiro Nome',
                      ),
                      initialValue: this.firstName,
                      onChanged: (fn) {
                        widget.firstNameUpdated(fn);
                      },
                    ),
                    SizedBox(
                      height: 10.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                        labelText: 'Último Nome',
                      ),
                      initialValue: this.lastName,
                      onChanged: (ln) {
                        widget.lastNameUpdated(ln);
                      },
                    ),
                    SizedBox(
                      height: 10.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                        labelText: 'Nome de Utilizador',
                      ),
                      initialValue: this.username,
                      onChanged: (un) {
                        widget.usernameUpdated(un);
                      },
                    ),
                    SizedBox(
                      height: 10.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                        labelText: 'Rua',
                      ),
                      initialValue: this.address.split(",").first,
                      onChanged: (street) {
                        widget.streetUpdated(street);
                      },
                    ),
                    SizedBox(
                      height: 10.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                        labelText: 'Número de Porta',
                      ),
                      initialValue: RegExp(r"[0-9]\S+")
                          .allMatches(this.address)
                          .elementAt(0)
                          .group(0),
                      keyboardType: TextInputType.number,
                      onChanged: (streetNumber) {
                        widget.streetNumberUpdated(int.parse(streetNumber));
                      },
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                        labelText: 'Código postal',
                      ),
                      initialValue: RegExp(r"[0-9]\S+")
                          .allMatches(this.address)
                          .elementAt(1)
                          .group(0),
                      onChanged: (postalCode) {
                        widget.postalCodeUpdated(postalCode);
                      },
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                        labelText: 'Cidade',
                      ),
                      initialValue: RegExp(r"[0-9] ([a-zA-Z]*),")
                          .allMatches(this.address)
                          .elementAt(0)
                          .group(1),
                      onChanged: (district) {
                        widget.districtUpdated(district);
                      },
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                        labelText: 'País',
                      ),
                      initialValue: this.address.split(",").last.trim(),
                      onChanged: (country) {
                        widget.countryUpdated(country);
                      },
                    ),
                    SizedBox(
                      height: 10.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                        labelText: 'Biografia',
                      ),
                      initialValue: this.bio,
                      onChanged: (des) {
                        widget.bioUpdated(des);
                      },
                    ),
                    SizedBox(
                      height: 10.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                        labelText: 'Distância máxima',
                      ),
                      initialValue: "30",
                      keyboardType: TextInputType.number,
                      onChanged: (rng) {
                        widget.rangeUpdated(int.parse(rng));
                      },
                    ),
                  ],
                ),
              ),
            )
          ],
        ),
      );
    } else {
      return SingleChildScrollView(
        child: Column(
          children: <Widget>[
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
              child: Container(
                width: double.infinity,
                height: 150,
                child: Container(
                  alignment: Alignment(0.0, 15.0),
                  child: CircleAvatar(
                    radius: 70.0,
                    child: CircleAvatar(
                      child: Align(
                        alignment: Alignment.bottomRight,
                        child: CircleAvatar(
                          backgroundColor: Color(0xFF006064),
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
                          "https://icon-library.com/images/facebook-user-icon/facebook-user-icon-4.jpg"),
                      radius: 68.0,
                    ),
                  ),
                ),
              ),
            ),
            Container(
              child: Padding(
                padding: EdgeInsets.symmetric(vertical: 10.0, horizontal: 10.0),
                child: Column(
                  children: <Widget>[
                    SizedBox(
                      height: 85.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: 'Primeiro Nome',
                          labelStyle: TextStyle(
                            color: Color(0xFF006064),
                          ),
                          focusedBorder: new UnderlineInputBorder(
                              borderSide:
                                  new BorderSide(color: Color(0xFF006064)))),
                      initialValue: this.firstName,
                      onChanged: (fn) {
                        widget.firstNameUpdated(fn);
                      },
                    ),
                    SizedBox(
                      height: 10.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: 'Último Nome',
                          labelStyle: TextStyle(
                            color: Color(0xFF006064),
                          ),
                          focusedBorder: new UnderlineInputBorder(
                              borderSide:
                                  new BorderSide(color: Color(0xFF006064)))),
                      initialValue: this.lastName,
                      onChanged: (ln) {
                        widget.lastNameUpdated(ln);
                      },
                    ),
                    SizedBox(
                      height: 10.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: 'Nome de Utilizador',
                          labelStyle: TextStyle(
                            color: Color(0xFF006064),
                          ),
                          focusedBorder: new UnderlineInputBorder(
                              borderSide:
                                  new BorderSide(color: Color(0xFF006064)))),
                      initialValue: this.username,
                      onChanged: (un) {
                        widget.usernameUpdated(un);
                      },
                    ),
                    SizedBox(
                      height: 10.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: 'Rua',
                          labelStyle: TextStyle(
                            color: Color(0xFF006064),
                          ),
                          focusedBorder: new UnderlineInputBorder(
                              borderSide:
                                  new BorderSide(color: Color(0xFF006064)))),
                      initialValue: this.address.split(",").first,
                      onChanged: (street) {
                        widget.streetUpdated(street);
                      },
                    ),
                    SizedBox(
                      height: 10.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: 'Número de Porta',
                          labelStyle: TextStyle(
                            color: Color(0xFF006064),
                          ),
                          focusedBorder: new UnderlineInputBorder(
                              borderSide:
                                  new BorderSide(color: Color(0xFF006064)))),
                      initialValue: RegExp(r"[0-9]\S+")
                          .allMatches(this.address)
                          .elementAt(0)
                          .group(0),
                      keyboardType: TextInputType.number,
                      onChanged: (streetNumber) {
                        widget.streetNumberUpdated(int.parse(streetNumber));
                      },
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: 'Código postal',
                          labelStyle: TextStyle(
                            color: Color(0xFF006064),
                          ),
                          focusedBorder: new UnderlineInputBorder(
                              borderSide:
                                  new BorderSide(color: Color(0xFF006064)))),
                      initialValue: RegExp(r"[0-9]\S+")
                          .allMatches(this.address)
                          .elementAt(1)
                          .group(0),
                      onChanged: (postalCode) {
                        widget.postalCodeUpdated(postalCode);
                      },
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: 'Cidade',
                          labelStyle: TextStyle(
                            color: Color(0xFF006064),
                          ),
                          focusedBorder: new UnderlineInputBorder(
                              borderSide:
                                  new BorderSide(color: Color(0xFF006064)))),
                      initialValue: RegExp(r"[0-9] ([a-zA-Z]*),")
                          .allMatches(this.address)
                          .elementAt(0)
                          .group(1),
                      onChanged: (district) {
                        widget.districtUpdated(district);
                      },
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: 'País',
                          labelStyle: TextStyle(
                            color: Color(0xFF006064),
                          ),
                          focusedBorder: new UnderlineInputBorder(
                              borderSide:
                                  new BorderSide(color: Color(0xFF006064)))),
                      initialValue: this.address.split(",").last.trim(),
                      onChanged: (country) {
                        widget.countryUpdated(country);
                      },
                    ),
                    SizedBox(
                      height: 10.0,
                    ),
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: 'Biografia',
                          labelStyle: TextStyle(
                            color: Color(0xFF006064),
                          ),
                          focusedBorder: new UnderlineInputBorder(
                              borderSide:
                                  new BorderSide(color: Color(0xFF006064)))),
                      initialValue: this.bio,
                      onChanged: (des) {
                        widget.bioUpdated(des);
                      },
                    ),
                  ],
                ),
              ),
            )
          ],
        ),
      );
    }
  }
}
