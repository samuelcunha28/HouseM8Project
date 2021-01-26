import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/enums/categories.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/work_categories.dart';
import 'package:housem8_flutter/view_models/categories_list_view_model.dart';
import 'package:housem8_flutter/view_models/work_categories_view_model.dart';
import 'package:housem8_flutter/widgets/add_category_alert_dialog.dart';
import 'package:housem8_flutter/widgets/categories_list.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:provider/provider.dart';

class WorkCategoriesScreen extends StatefulWidget {
  @override
  _WorkCategoriesScreenState createState() => _WorkCategoriesScreenState();
}

class _WorkCategoriesScreenState extends State<WorkCategoriesScreen> {
  bool isDeviceConnected = true;
  List<WorkCategoriesViewModel> categories = List<WorkCategoriesViewModel>();
  var internetConnection;

  Widget categoriesList = Container();

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
        appBar: AppBar(
          backgroundColor: Color(0xFF39A3ED),
          title: Text('Categorias de trabalho'),
          actions: <Widget>[
            IconButton(
              icon: Icon(Icons.add),
              iconSize: 35.0,
              tooltip: 'Adicionar categoria',
              //Ao carregar, abre um Dialog
              onPressed: () {
                showDialog(
                        context: context,
                        builder: (context) => AddCategoryAlertDialog())
                    .then((value) {
                  if (value != null) {
                    WorkCategories newCategory = WorkCategories(
                        category:
                            EnumToString.fromString(Categories.values, value));
                    updateDataFromService(newCategory);
                  }
                });
              },
            ),
          ],
        ),
        body: Container(
          padding: EdgeInsets.all(10),
          width: MediaQuery.of(context).size.width,
          height: MediaQuery.of(context).size.height,
          child: Column(
            children: <Widget>[
              Expanded(child: categoriesList),
            ],
          ),
        ),
      );
    } else {
      return Scaffold(
          appBar: AppBar(title: Text("Categorias de trabalho")),
          body: OfflineMessage());
    }
  }

  void getDataFromService() {
    final vm = Provider.of<CategoriesListViewModel>(context, listen: false);
    categoriesList = Container();
    setState(() {});
    vm.fetchWorkCategories().then((value) {
      categories = vm.list;
      categoriesList = CategoriesList(categories);
      setState(() {});
    });
  }

  void updateDataFromService(WorkCategories workCategory) {
    final vm = Provider.of<CategoriesListViewModel>(context, listen: false);
    vm.addCategory(workCategory).then((value) => {getDataFromService()});
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
