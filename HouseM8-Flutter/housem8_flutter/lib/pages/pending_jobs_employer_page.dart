import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/view_models/pending_job_list_view_model.dart';
import 'package:housem8_flutter/view_models/pending_job_view_model.dart';
import 'package:housem8_flutter/widgets/employer_app_bar.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:housem8_flutter/widgets/pending_jobs_list.dart';
import 'package:provider/provider.dart';

class PendingJobsEmployerPage extends StatefulWidget {
  @override
  _PendingJobsEmployerPageState createState() =>
      _PendingJobsEmployerPageState();
}

class _PendingJobsEmployerPageState extends State<PendingJobsEmployerPage> {
  List<PendingJobViewModel> pendingJobs = List<PendingJobViewModel>();
  bool isDeviceConnected = true;
  var internetConnection;

  Widget pendingList = Container();

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
        appBar: EmployerAppBar("Trabalhos Marcados", false),
        body: Container(
          padding: EdgeInsets.all(10),
          width: MediaQuery.of(context).size.width,
          height: MediaQuery.of(context).size.height,
          child: Column(
            children: <Widget>[
              Expanded(child: pendingList),
            ],
          ),
        ),
      );
    } else {
      return Scaffold(
          appBar: EmployerAppBar("Trabalhos Marcados", false),
          body: OfflineMessage());
    }
  }

  void getDataFromService() {
    final vm = Provider.of<PendingJobListViewModel>(context, listen: false);
    vm.fetchEmployerPendingJob().then((value) {
      pendingJobs = vm.pendingJobs;
      pendingList = PendingJobsList(pendingJobs: pendingJobs, role: "EMPLOYER");
      setState(() {});
    });
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
