import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/view_models/reviews_list_view_model.dart';
import 'package:housem8_flutter/view_models/reviews_view_model.dart';
import 'package:housem8_flutter/widgets/mate_app_bar.dart';
import 'package:housem8_flutter/widgets/mate_reviews_list.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:provider/provider.dart';

class MateReviewsPage extends StatefulWidget {
  @override
  _MateReviewsPageState createState() => _MateReviewsPageState();
}

class _MateReviewsPageState extends State<MateReviewsPage> {
  List<ReviewsViewModel> mateReviews = List<ReviewsViewModel>();
  bool isDeviceConnected = true;
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
        appBar: MateAppBar("Reviews", false),
        body: Container(
          padding: EdgeInsets.all(10),
          width: MediaQuery.of(context).size.width,
          height: MediaQuery.of(context).size.height,
          child: Column(
            children: <Widget>[
              Expanded(
                  child: MateReviewsList(
                mateReviews: mateReviews,
              ))
            ],
          ),
        ),
      );
    } else {
      return Scaffold(
        appBar: MateAppBar("Reviews", false),
        body: OfflineMessage(),
      );
    }
  }

  void getDataFromService() {
    final vm = Provider.of<ReviewsListViewModel>(context, listen: false);
    vm.fetchMateReviews().then((value) {
      mateReviews = vm.reviews;
      setState(() {});
    });
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
