import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/view_models/favorite_mates_list_view_model.dart';
import 'package:housem8_flutter/view_models/favorite_mates_view_model.dart';
import 'package:housem8_flutter/widgets/employer_app_bar.dart';
import 'package:housem8_flutter/widgets/favorite_mates_list.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:provider/provider.dart';

class FavoriteMatesScreen extends StatefulWidget {
  @override
  _FavoriteMatesScreenState createState() => _FavoriteMatesScreenState();
}

class _FavoriteMatesScreenState extends State<FavoriteMatesScreen> {
  List<FavoriteMatesViewModel> favoriteMates = List<FavoriteMatesViewModel>();
  bool isDeviceConnected = true;
  var internetConnection;

  Widget favoriteList = Container();

  @override
  void initState() {
    super.initState();

    //Verificar Conectividade
    ConnectionHelper.checkConnection().then((value) {
      isDeviceConnected = value;
      if (isDeviceConnected) {
        getDataFromService();
      } else {
        setState(() {});
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
        appBar: EmployerAppBar("Mates Favoritos", false),
        body: Container(
          child: Column(
            children: <Widget>[
              Expanded(child: favoriteList),
            ],
          ),
        ),
      );
    } else {
      return Scaffold(
          appBar: EmployerAppBar("Mates Favoritos", false),
          body: OfflineMessage());
    }
  }

  void getDataFromService() {
    final vm = Provider.of<FavoriteMatesListViewModel>(context, listen: false);
    vm.fetchFavoriteMate().then((value) {
      favoriteMates = vm.favoriteMates;
      favoriteList = FavoriteMatesList(favoriteMates);
      setState(() {});
    });
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
