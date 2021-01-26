import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/job_post_publication.dart';
import 'package:housem8_flutter/pages/create_job_post_page.dart';
import 'package:housem8_flutter/pages/update_job_post_page.dart';
import 'package:housem8_flutter/view_models/employer_post_list_view.dart';
import 'package:housem8_flutter/view_models/employer_post_view.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:housem8_flutter/widgets/remove_alert_dialog.dart';
import 'package:provider/provider.dart';

class EmployerPostsListPage extends StatefulWidget {
  @override
  _EmployerPostsListPageState createState() => _EmployerPostsListPageState();
}

class _EmployerPostsListPageState extends State<EmployerPostsListPage> {
  List<EmployerPostViewModel> employerPosts = List<EmployerPostViewModel>();
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
        appBar: getAppBar(),
        body: Container(
          padding: EdgeInsets.only(left: 12.0, top: 10.0, right: 12.0),
          child: Column(
            children: <Widget>[
              Expanded(
                  child: ListView.builder(
                      itemCount: this.employerPosts.length,
                      itemBuilder: (context, index) {
                        final post = this.employerPosts[index];
                        return Card(
                          child: ListTile(
                            title: Text(
                              post.title,
                              style: TextStyle(
                                  fontSize: 16.0,
                                  color: Color(Colors.black.value),
                                  fontWeight: FontWeight.w700),
                            ),
                            subtitle: Text(
                              post.description,
                              style: TextStyle(
                                  fontSize: 14.0,
                                  color: Color(Colors.black.value)),
                            ),
                            trailing: Row(
                                mainAxisSize: MainAxisSize.min,
                                children: <Widget>[
                                  IconButton(
                                    icon: Icon(Icons.edit),
                                    color: Color(0xFF006064),
                                    iconSize: 30,
                                    tooltip: 'Editar Trabalho',
                                    onPressed: () async {
                                      await Navigator.push(
                                          context,
                                          MaterialPageRoute(
                                              builder: (context) =>
                                                  ChangeNotifierProvider(
                                                    create: (context) =>
                                                        EmployerPostListViewModel(),
                                                    child: UpdateJobPostPage(
                                                        post: post),
                                                  ))).then((value) {
                                        getDataFromService();
                                      });
                                    },
                                  ),
                                  IconButton(
                                    icon: Icon(Icons.delete),
                                    color: Color(0xFF006064),
                                    iconSize: 30,
                                    tooltip: 'Apagar Trabalho!',
                                    onPressed: () async {
                                      showDialog(
                                          context: context,
                                          builder: (context) => RemoveAlertDialog(
                                              "Remover Publicação de Trabalho",
                                              "Tem a certeza que quer remover a publicação ?")).then(
                                          (value) async {
                                        if (value == "Sim") {
                                          await deleteJobPost(post.id);
                                        }
                                      });
                                    },
                                  ),
                                ]),
                          ),
                        );
                      })),
            ],
          ),
        ),
      );
    } else {
      return Scaffold(appBar: getAppBar(), body: OfflineMessage());
    }
  }

  void getDataFromService() {
    final vm = Provider.of<EmployerPostListViewModel>(context, listen: false);
    vm.fetchEmployerPosts().then((value) {
      employerPosts = vm.employerPosts;
      setState(() {});
    });
  }

  Future<void> createJobPost(JobPostPublication post) async {
    if (post != null) {
      final vm = Provider.of<EmployerPostListViewModel>(context, listen: false);
      await vm.addEmployerPost(post);
      getDataFromService();
    }
  }

  Future<void> deleteJobPost(int id) async {
    final vm = Provider.of<EmployerPostListViewModel>(context, listen: false);
    await vm.deleteEmployerPost(id);
    getDataFromService();
  }

  Widget getAppBar() {
    return AppBar(
      centerTitle: true,
      title: Text("Publicações de Trabalhos"),
      backgroundColor: Color(0xFF93C901),
      actions: <Widget>[
        IconButton(
          icon: Icon(Icons.add),
          tooltip: 'Adicionar Publicação',
          onPressed: () async {
            final information = await Navigator.push(
              context,
              MaterialPageRoute(
                  fullscreenDialog: true,
                  builder: (context) => CreateJobPostPage()),
            );
            await createJobPost(information);
          },
        ),
      ],
    );
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
