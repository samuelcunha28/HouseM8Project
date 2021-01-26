import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/view_models/mate_profile_view_model.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:housem8_flutter/widgets/profile.dart';
import 'package:provider/provider.dart';

class MateProfileScreen extends StatefulWidget {
  @override
  _MateProfileScreenState createState() => _MateProfileScreenState();
}

class _MateProfileScreenState extends State<MateProfileScreen> {
  MateProfileViewModel profile = MateProfileViewModel();
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
          Profile("MATE", profile.firstName, profile.lastName, profile.address,
              profile.description, profile.averageRating),
        ]),
      );
    } else {
      return Scaffold(
        body: OfflineMessage(),
      );
    }
  }

  void getDataFromService() {
    final vm = Provider.of<MateProfileViewModel>(context, listen: false);
    vm.mateProfileViewModelConstructor().then((value) {
      profile = vm;
      setState(() {});
    });
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
