import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/profile_model.dart';
import 'package:housem8_flutter/view_models/employer_profile_view_model.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:housem8_flutter/widgets/profile.dart';
import 'package:provider/provider.dart';

class EmployerProfileScreen extends StatefulWidget {
  @override
  _EmployerProfileScreenState createState() => _EmployerProfileScreenState();
}

class _EmployerProfileScreenState extends State<EmployerProfileScreen> {
  ProfileModel profile = ProfileModel(
      id: 0,
      firstName: "...",
      lastName: "...",
      address: "...",
      averageRating: 0,
      email: "...",
      userName: "...");
  bool isDeviceConnected = false;
  var internetConnection;

  @override
  void initState() {
    super.initState();

    //Verificar Conectividade
    ConnectionHelper.checkConnection().then((value) {
      isDeviceConnected = value;
      if (isDeviceConnected) {
        getDataFromService();
      }
    });

    //Ativar listener para caso a conectividade mude
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
        body: Stack(children: <Widget>[
          Profile("EMPLOYER", profile.firstName, profile.lastName,
              profile.address, profile.description, profile.averageRating),
        ]),
      );
    } else {
      return Scaffold(
        body: OfflineMessage(),
      );
    }
  }

  void getDataFromService() {
    final vm = Provider.of<EmployerProfileViewModel>(context, listen: false);
    vm.fetchEmployerProfile().then((value) {
      profile = vm.employerProfile;
      setState(() {});
    });
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
