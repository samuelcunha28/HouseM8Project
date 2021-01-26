import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/address.dart';
import 'package:housem8_flutter/models/mate_update.dart';
import 'package:housem8_flutter/view_models/mate_profile_view_model.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:housem8_flutter/widgets/update_users.dart';
import 'package:provider/provider.dart';

class UpdateMatePage extends StatefulWidget {
  @override
  _UpdateMatePageState createState() => _UpdateMatePageState();
}

class _UpdateMatePageState extends State<UpdateMatePage> {
  MateProfileViewModel vm;
  MateUpdate update = MateUpdate(address: Address());
  bool isDeviceConnected = false;
  var internetConnection;

  @override
  void initState() {
    super.initState();
    vm = Provider.of<MateProfileViewModel>(context, listen: false);

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
            backgroundColor: Color(0xFF39A3ED),
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
            role: "MATE",
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
            rangeUpdated: (rng) {
              update.range = rng;
            },
          ));
    } else {
      return Scaffold(
        appBar: AppBar(
          centerTitle: true,
          title: Text("Editar Perfil"),
          backgroundColor: Color(0xFF39A3ED),
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
    vm.mateProfileViewModelConstructor().then((value) {
      setState(() {});
    });
  }

  void updateDataFromService(MateUpdate mateUpdate) {
    if (mateUpdate.firstName == null) {
      mateUpdate.firstName = vm.firstName;
    }

    if (mateUpdate.lastName == null) {
      mateUpdate.lastName = vm.lastName;
    }

    if (mateUpdate.userName == null) {
      mateUpdate.userName = vm.userName;
    }

    if (mateUpdate.address.street == null) {
      mateUpdate.address.street = vm.address.split(",").first;
    }

    if (mateUpdate.address.streetNumber == null) {
      mateUpdate.address.streetNumber = int.parse(
          RegExp(r"[0-9]\S+").allMatches(vm.address).elementAt(0).group(0));
    }

    if (mateUpdate.address.postalCode == null) {
      mateUpdate.address.postalCode =
          RegExp(r"[0-9]\S+").allMatches(vm.address).elementAt(1).group(0);
    }

    if (mateUpdate.address.district == null) {
      mateUpdate.address.district = RegExp(r"[0-9] ([a-zA-Z]*),")
          .allMatches(vm.address)
          .elementAt(0)
          .group(1);
    }

    if (mateUpdate.address.country == null) {
      mateUpdate.address.country = vm.address.split(",").last.trim();
    }

    if (mateUpdate.description == null) {
      mateUpdate.description = vm.description;
    }

    if (mateUpdate.range == null) {
      mateUpdate.range = 30;
    }

    vm.updateMateProfile(mateUpdate);
  }

  @override
  void dispose() {
    super.dispose();
    internetConnection.cancel();
  }
}
