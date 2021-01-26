import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/address.dart';
import 'package:housem8_flutter/models/employer_update.dart';
import 'package:housem8_flutter/view_models/employer_profile_view_model.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:housem8_flutter/widgets/update_users.dart';
import 'package:provider/provider.dart';

class UpdateEmployerPage extends StatefulWidget {
  @override
  _UpdateEmployerPageState createState() => _UpdateEmployerPageState();
}

class _UpdateEmployerPageState extends State<UpdateEmployerPage> {
  EmployerProfileViewModel vm;
  EmployerUpdate update = EmployerUpdate(address: Address());
  bool isDeviceConnected = false;
  var internetConnection;

  @override
  void initState() {
    super.initState();
    vm = Provider.of<EmployerProfileViewModel>(context, listen: false);

    ConnectionHelper.checkConnection().then((value) {
      isDeviceConnected = value;
      if (isDeviceConnected) {
        getDataFromService();
      }
    });

    internetConnection = Connectivity()
        .onConnectivityChanged
        .listen((ConnectivityResult result) async {
      if (result != ConnectivityResult.none) {
        isDeviceConnected = await DataConnectionChecker().hasConnection;
        if (isDeviceConnected) {
          getDataFromService();
        }
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
          title: Text("Editar Perfil"),
          backgroundColor: Color(0xFF93C901),
          actions: <Widget>[
            IconButton(
              icon: Icon(Icons.save),
              tooltip: 'Guardar alterações',
              onPressed: () {
                updateDataFromService(update);
                Navigator.of(context).pop();
              },
            ),
          ],
        ),
        body: UpdateUsers(
          role: "EMPLOYER",
          firstName: vm.firstName,
          lastName: vm.lastName,
          username: vm.userName,
          address: vm.address,
          bio: vm.description,
          firstNameUpdated: (fName) {
            update.firstName = fName;
          },
          lastNameUpdated: (lName) {
            update.lastName = lName;
          },
          usernameUpdated: (uName) {
            update.userName = uName;
          },
          streetUpdated: (str) {
            update.address.street = str;
          },
          streetNumberUpdated: (strn) {
            update.address.streetNumber = strn;
          },
          postalCodeUpdated: (pc) {
            update.address.postalCode = pc;
          },
          districtUpdated: (dis) {
            update.address.district = dis;
          },
          countryUpdated: (co) {
            update.address.country = co;
          },
          bioUpdated: (bio) {
            update.description = bio;
          },
        ),
      );
    } else {
      return Scaffold(
        appBar: AppBar(
          centerTitle: true,
          title: Text("Editar Perfil"),
          backgroundColor: Color(0xFF93C901),
          actions: <Widget>[
            IconButton(
              icon: Icon(Icons.save),
              tooltip: 'Guardar alterações',
              onPressed: () {
                updateDataFromService(update);
                Navigator.of(context).pop();
              },
            ),
          ],
        ),
        body: OfflineMessage(),
      );
    }
  }

  void getDataFromService() {
    vm.fetchEmployerProfile().then((value) {
      setState(() {});
    });
  }

  void updateDataFromService(EmployerUpdate employerUpdate) {
    if (employerUpdate.firstName == null) {
      employerUpdate.firstName = vm.firstName;
    }

    if (employerUpdate.lastName == null) {
      employerUpdate.lastName = vm.lastName;
    }

    if (employerUpdate.userName == null) {
      employerUpdate.userName = vm.userName;
    }

    if (employerUpdate.address.street == null) {
      employerUpdate.address.street = vm.address.split(",").first;
    }

    if (employerUpdate.address.streetNumber == null) {
      employerUpdate.address.streetNumber = int.parse(
          RegExp(r"[0-9]\S+").allMatches(vm.address).elementAt(0).group(0));
    }

    if (employerUpdate.address.postalCode == null) {
      employerUpdate.address.postalCode =
          RegExp(r"[0-9]\S+").allMatches(vm.address).elementAt(1).group(0);
    }

    if (employerUpdate.address.district == null) {
      employerUpdate.address.district = RegExp(r"[0-9] ([a-zA-Z]*),")
          .allMatches(vm.address)
          .elementAt(0)
          .group(1);
    }

    if (employerUpdate.address.country == null) {
      employerUpdate.address.country = vm.address.split(",").last.trim();
    }

    if (employerUpdate.description == null) {
      employerUpdate.description = vm.description;
    }

    vm.updateEmployerProfile(employerUpdate);
  }

  @override
  void dispose() {
    super.dispose();
    internetConnection.cancel();
  }
}
