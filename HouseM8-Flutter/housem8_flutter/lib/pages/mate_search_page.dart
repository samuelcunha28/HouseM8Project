import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/view_models/mate_list_view_model.dart';
import 'package:housem8_flutter/view_models/mate_view_model.dart';
import 'package:housem8_flutter/widgets/mate_search_widget.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:provider/provider.dart';

class MateSearchPage extends StatefulWidget {
  @override
  _MateSearchPageState createState() => _MateSearchPageState();
}

class _MateSearchPageState extends State<MateSearchPage> {
  List<MateViewModel> matesList = List<MateViewModel>();
  bool isDeviceConnected = false;
  double height;
  double width;
  Widget matesCards;
  var internetConnection;

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
    height = MediaQuery.of(context).size.height;
    width = MediaQuery.of(context).size.width;
    if (isDeviceConnected) {
      if (matesList.length > 0) {
        return Scaffold(
            body: Container(
          padding: EdgeInsets.all(10),
          child: SingleChildScrollView(
              child: Column(
            children: [
              Container(height: height / 1.45, child: matesCards),
              getButtons()
            ],
          )),
        ));
      } else {
        return Scaffold(
          body: Container(
              padding: EdgeInsets.all(10),
              child: SingleChildScrollView(
                  child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  Container(
                    height: height / 1.492,
                    child: Center(
                        child: Text(
                      "Não existem Mates com estes critérios!",
                      style:
                          TextStyle(fontWeight: FontWeight.w700, fontSize: 15),
                    )),
                  ),
                  getButtons()
                ],
              ))),
        );
      }
    } else {
      return OfflineMessage();
    }
  }

  void getDataFromService() {
    final vm = Provider.of<MateListViewModel>(context, listen: false);
    matesCards = null;
    setState(() {});
    vm.fetchMates().then((value) {
      matesList = vm.mates;
      matesCards = MateSearchWidget(matesList: vm.mates);
      setState(() {});
    });
  }

  Widget getButtons() {
    return Container(
        width: width / 1.2,
        child: Row(
          children: [
            Container(
              width: width / 1.52,
              decoration: BoxDecoration(
                borderRadius: BorderRadius.only(
                    topLeft: Radius.circular(15),
                    topRight: Radius.circular(15),
                    bottomLeft: Radius.circular(15),
                    bottomRight: Radius.circular(15)),
                boxShadow: [
                  BoxShadow(
                      color: Colors.black54,
                      blurRadius: 1.0,
                      offset: Offset(0.0, 0.5))
                ],
              ),
              child: ButtonTheme(
                minWidth: width / 1.52,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(15),
                ),
                height: height / 14,
                child: RaisedButton(
                  child: Text(
                    'Filtros',
                    style: TextStyle(fontWeight: FontWeight.bold, fontSize: 16),
                  ),
                  color: Colors.white,
                  onPressed: () {
                    print("Filtros");
                  },
                ),
              ),
            ),
            SizedBox(
              width: 15,
            ),
            Container(
              width: width / 7.5,
              decoration: BoxDecoration(
                borderRadius: BorderRadius.only(
                    topLeft: Radius.circular(15),
                    topRight: Radius.circular(15),
                    bottomLeft: Radius.circular(15),
                    bottomRight: Radius.circular(15)),
                boxShadow: [
                  BoxShadow(
                      color: Colors.black54,
                      blurRadius: 1.0,
                      offset: Offset(0.0, 0.5))
                ],
              ),
              child: ButtonTheme(
                minWidth: width / 7.5,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(15),
                ),
                height: height / 14,
                child: RaisedButton(
                  child: Icon(Icons.refresh),
                  color: Colors.white,
                  onPressed: () {
                    getDataFromService();
                  },
                ),
              ),
            ),
          ],
        ));
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
