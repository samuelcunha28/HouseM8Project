import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/work.dart';
import 'package:housem8_flutter/view_models/employer_post_list_view.dart';
import 'package:housem8_flutter/view_models/employer_post_view.dart';
import 'package:housem8_flutter/view_models/work_view_model.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:provider/provider.dart';

class WorkCreatePage extends StatefulWidget {
  final int mateId;

  const WorkCreatePage({Key key, this.mateId}) : super(key: key);

  @override
  _WorkCreatePageState createState() => _WorkCreatePageState();
}

class _WorkCreatePageState extends State<WorkCreatePage> {
  final TextEditingController _dateController = TextEditingController();
  final TextEditingController _timeController = TextEditingController();
  List<EmployerPostViewModel> employerPosts = List<EmployerPostViewModel>();

  WorkViewModel vm;
  Work newWork = Work();
  bool isDeviceConnected = true;
  var internetConnection;

  DateTime selectedDate = DateTime.now();
  TimeOfDay selectedTime = TimeOfDay.now();
  EmployerPostViewModel jobPost = null;

  @override
  void initState() {
    super.initState();
    vm = Provider.of<WorkViewModel>(context, listen: false);

    //Verificar Conectividade
    ConnectionHelper.checkConnection().then((value) {
      isDeviceConnected = value;
      if (value) {
        getDataFromService();
      }
      setState(() {});
    });

    //Ativar listener para caso a conectividade mude
    internetConnection = Connectivity()
        .onConnectivityChanged
        .listen((ConnectivityResult result) async {
      if (result != ConnectivityResult.none) {
        isDeviceConnected = await DataConnectionChecker().hasConnection;
        setState(() {});
      } else {
        isDeviceConnected = false;
        setState(() {});
      }
    });
  }

  Future<void> _selectDate(BuildContext context) async {
    final DateTime picked = await showDatePicker(
        context: context,
        locale: const Locale("pt", "PT"),
        initialDate: selectedDate,
        firstDate: DateTime(2015, 8),
        lastDate: DateTime(2101),
        builder: (BuildContext context, Widget child) {
          return Theme(
            data: ThemeData.light().copyWith(
              colorScheme: ColorScheme.fromSwatch(
                primarySwatch: Colors.lightGreen,
                primaryColorDark: Colors.lightGreen,
                accentColor: Colors.lightGreen,
              ),
              dialogBackgroundColor: Colors.white,
            ),
            child: child,
          );
        });
    if (picked != null && picked != selectedDate)
      setState(() {
        selectedDate = picked;
        _dateController.text = selectedDate.toString();
      });
  }

  Future<void> _selectTime(BuildContext context) async {
    final TimeOfDay pickedTime = await showTimePicker(
        context: context,
        initialTime: selectedTime,
        builder: (BuildContext context, Widget child) {
          return Theme(
            data: ThemeData.light().copyWith(
              colorScheme: ColorScheme.fromSwatch(
                primarySwatch: Colors.lightGreen,
                primaryColorDark: Colors.lightGreen,
                accentColor: Colors.lightGreen,
              ),
              dialogBackgroundColor: Colors.white,
            ),
            child: child,
          );
        });

    if (pickedTime != null && pickedTime != selectedTime) {
      setState(() {
        selectedTime = pickedTime;
        _timeController.text = selectedTime.toString();
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    if (isDeviceConnected) {
      return Scaffold(
          appBar: AppBar(
            centerTitle: true,
            title: Text("Criar Trabalho"),
            backgroundColor: Color(0xFF93C901),
            actions: <Widget>[
              IconButton(
                icon: Icon(Icons.save),
                tooltip: 'Guardar criação de trabalho',
                onPressed: () {
                  save();
                  Navigator.of(context).pop();
                },
              ),
            ],
          ),
          body: SingleChildScrollView(
              child: Padding(
            padding: EdgeInsets.symmetric(vertical: 10.0, horizontal: 10.0),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: <Widget>[
                SizedBox(
                  height: 120,
                ),
                Text(
                  "Selecione a publicação de trabalho",
                  style: TextStyle(fontSize: 20, fontWeight: FontWeight.w600),
                ),
                DropdownButtonFormField<EmployerPostViewModel>(
                  decoration: InputDecoration(
                    labelText: 'Publicação',
                    labelStyle: TextStyle(color: Colors.black),
                  ),
                  value: jobPost,
                  icon: Icon(Icons.arrow_drop_down),
                  iconSize: 24,
                  isExpanded: true,
                  elevation: 16,
                  items: employerPosts.map((EmployerPostViewModel post) {
                    return DropdownMenuItem<EmployerPostViewModel>(
                      value: post,
                      child: Text(post.title),
                    );
                  }).toList(),
                  onChanged: (EmployerPostViewModel newValue) {
                    setState(() {
                      jobPost = newValue;
                    });
                  },
                ),
                SizedBox(
                  height: 50,
                ),
                Text(
                  "Selecione a data a realizar o trabalho",
                  style: TextStyle(fontSize: 20, fontWeight: FontWeight.w600),
                ),
                Row(
                  children: <Widget>[
                    IconButton(
                      icon: Icon(Icons.calendar_today),
                      onPressed: () => _selectDate(context),
                    ),
                    Text(
                      "Data: ${selectedDate.day}/${selectedDate.month}/${selectedDate.year}",
                      style: TextStyle(fontSize: 18),
                    ),
                  ],
                ),
                SizedBox(
                  height: 50,
                ),
                Text(
                  "Selecione a hora a realizar o trabalho",
                  style: TextStyle(fontSize: 20, fontWeight: FontWeight.w600),
                ),
                Row(
                  children: <Widget>[
                    IconButton(
                      icon: Icon(Icons.access_time),
                      onPressed: () => _selectTime(context),
                    ),
                    Text(
                      "Hora: ${selectedTime.hour}:${selectedTime.minute}",
                      style: TextStyle(fontSize: 18),
                    ),
                  ],
                ),
              ],
            ),
          )));
    } else {
      return Scaffold(
          appBar: AppBar(
            centerTitle: true,
            title: Text("Criar Trabalho"),
            backgroundColor: Color(0xFF93C901),
            actions: <Widget>[
              IconButton(
                icon: Icon(Icons.save),
                tooltip: 'Guardar criação de trabalho',
                onPressed: () {
                  showDialog(
                    context: context,
                    builder: (context) => ErrorMessageDialog(
                        title: "Dispositivo Offline!",
                        text: "O dispositivo não está conectado à internet!"),
                  );
                },
              ),
            ],
          ),
          body: OfflineMessage());
    }
  }

  void getDataFromService() {
    final vm = EmployerPostListViewModel();
    vm.fetchEmployerPosts().then((value) {
      employerPosts = vm.employerPosts;
      setState(() {});
    });
  }

  void save() {
    if (widget.mateId != null) {
      newWork.mateId = widget.mateId;
    }

    if (jobPost != null) {
      newWork.jobPostId = jobPost.id;
    } else {
      newWork.jobPostId = employerPosts.first.id;
    }

    if (selectedDate.day == DateTime.now().day &&
        selectedDate.month == DateTime.now().month) {
      newWork.date = DateTime.now().add(Duration(hours: 24));
    } else {
      DateTime date = DateTime(selectedDate.year, selectedDate.month,
          selectedDate.day, selectedTime.hour, selectedTime.minute);
      newWork.date = date;
    }

    vm.addWork(newWork);
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
