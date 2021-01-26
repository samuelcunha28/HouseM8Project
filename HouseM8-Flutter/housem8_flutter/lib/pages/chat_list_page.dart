import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/enums/roles.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/view_models/chat_list_view_model.dart';
import 'package:housem8_flutter/view_models/chat_view_model.dart';
import 'package:housem8_flutter/widgets/chat_card.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:provider/provider.dart';

class ChatListPage extends StatefulWidget {
  @override
  _ChatListPageState createState() => _ChatListPageState();
}

class _ChatListPageState extends State<ChatListPage> {
  List<ChatViewModel> chatList = List<ChatViewModel>();
  bool isDeviceConnected = true;
  var internetConnection;
  Color color = Colors.black;

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

    StorageHelper.readTokenRole().then((value) {
      if (value == EnumToString.convertToString(Roles.M8)) {
        color = Color(0xFF39A3ED);
      } else {
        color = Color(0xFF93C901);
      }
      setState(() {});
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
      if (chatList.length > 0) {
        return Scaffold(
          body: Container(
            padding: EdgeInsets.only(left: 12.0, top: 10.0, right: 12.0),
            child: Column(
              children: <Widget>[
                Expanded(
                    child: ListView.builder(
                        itemCount: this.chatList.length,
                        itemBuilder: (context, index) {
                          final chat = this.chatList[index].chat;
                          return ChatCard(
                            color: color,
                            chat: chat,
                          );
                        })),
              ],
            ),
          ),
          floatingActionButton: FloatingActionButton(
              backgroundColor: color,
              onPressed: () {
                getDataFromService();
              },
              child: Icon(Icons.refresh)),
        );
      } else {
        return Scaffold(
          body: Container(
            child: Center(
              child: Text(
                "Não existem conversas atualmente!",
                style: TextStyle(fontWeight: FontWeight.w700, fontSize: 15),
              ),
            ),
          ),
          floatingActionButton: FloatingActionButton(
              backgroundColor: color,
              onPressed: () {
                getDataFromService();
              },
              child: Icon(Icons.refresh)),
        );
      }
    } else {
      return OfflineMessage();
    }
  }

  void getDataFromService() {
    final vm = Provider.of<ChatListViewModel>(context, listen: false);
    vm.fetchChats().then((value) {
      chatList = vm.chatList;
      setState(() {});
    });
  }

  @override
  void dispose() {
    internetConnection.cancel();
    super.dispose();
  }
}
