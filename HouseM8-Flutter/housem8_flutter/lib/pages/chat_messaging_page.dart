import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/enums/roles.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/helpers/messaging_helper.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/pages/work_create_page.dart';
import 'package:housem8_flutter/view_models/message_list_view_model.dart';
import 'package:housem8_flutter/view_models/message_view_model.dart';
import 'package:housem8_flutter/view_models/work_view_model.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';
import 'package:housem8_flutter/widgets/offline_message.dart';
import 'package:provider/provider.dart';

class ChatMessagingPage extends StatefulWidget {
  final int chatId;
  final int userId;
  final String userName;
  final int otherUserId;
  final String otherUserName;
  final Color color;

  const ChatMessagingPage(
      {Key key,
      this.chatId,
      this.color,
      this.otherUserName,
      this.otherUserId,
      this.userId,
      this.userName})
      : super(key: key);

  @override
  _ChatMessagingPageState createState() => _ChatMessagingPageState();
}

class _ChatMessagingPageState extends State<ChatMessagingPage> {
  final TextEditingController textEditingController =
      new TextEditingController();
  List<MessageViewModel> messageList = List<MessageViewModel>();
  bool isDeviceConnected = true;
  var internetConnection;
  Color color = Colors.black;
  var chatInstance = MessagingHelper();
  final ScrollController _controller = ScrollController();
  bool canStartWork = false;

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

    color = widget.color;

    StorageHelper.readTokenRole().then((value) {
      if (value.toString() == EnumToString.convertToString(Roles.EMPLOYER)) {
        canStartWork = true;
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
          centerTitle: true,
          title: Text(widget.otherUserName),
          backgroundColor: color,
        ),
        body: Container(
          padding: EdgeInsets.only(top: 10.0),
          child: Column(
            children: <Widget>[
              Expanded(
                  child: ListView.builder(
                      controller: _controller,
                      itemCount: this.messageList.length,
                      itemBuilder: (context, index) {
                        final message = this.messageList[index];
                        return Card(
                            elevation: 2.0,
                            margin: new EdgeInsets.symmetric(
                                horizontal: 15.0, vertical: 5),
                            child: (() {
                              if (message.senderId == widget.otherUserId) {
                                return ListTile(
                                  leading: CircleAvatar(
                                    backgroundImage: NetworkImage(
                                        "https://icon-library.com/images/facebook-user-icon/facebook-user-icon-4.jpg"), // no matter how big it is, it won't overflow
                                  ),
                                  title: Text(
                                    widget.otherUserName,
                                    style: TextStyle(
                                        fontSize: 16.0,
                                        color: Color(Colors.black.value),
                                        fontWeight: FontWeight.w700),
                                  ),
                                  subtitle: Text(
                                    message.messageSend,
                                    style: TextStyle(
                                        fontSize: 14.0,
                                        color: Color(Colors.black.value)),
                                  ),
                                );
                              } else {
                                return ListTile(
                                    trailing: CircleAvatar(
                                      backgroundImage: NetworkImage(
                                          "https://icon-library.com/images/facebook-user-icon/facebook-user-icon-4.jpg"), // no matter how big it is, it won't overflow
                                    ),
                                    title: Row(
                                      mainAxisAlignment: MainAxisAlignment.end,
                                      children: [
                                        Text(
                                          widget.userName,
                                          style: TextStyle(
                                              fontSize: 16.0,
                                              color: Color(Colors.black.value),
                                              fontWeight: FontWeight.w700),
                                        ),
                                      ],
                                    ),
                                    subtitle: Row(
                                        mainAxisAlignment:
                                            MainAxisAlignment.end,
                                        children: [
                                          Text(
                                            message.messageSend,
                                            style: TextStyle(
                                                fontSize: 14.0,
                                                color:
                                                    Color(Colors.black.value)),
                                          ),
                                        ]));
                              }
                            }()));
                      })),
              inputBar()
            ],
          ),
        ),
      );
    } else {
      return Scaffold(
          appBar: AppBar(
            centerTitle: true,
            title: Text(widget.otherUserName),
            backgroundColor: color,
          ),
          body: OfflineMessage());
    }
  }

  Widget inputBar() {
    return Container(
      child: Row(
        children: <Widget>[
          Material(
            child: new Container(
              margin: new EdgeInsets.symmetric(horizontal: 1.0),
              child: new IconButton(
                icon: new Icon(Icons.work),
                color: color,
                onPressed: () {
                  if (canStartWork) {
                    Navigator.of(context).push(MaterialPageRoute(
                        builder: (context) => ChangeNotifierProvider(
                              create: (context) => WorkViewModel(),
                              child: WorkCreatePage(
                                mateId: widget.otherUserId,
                              ),
                            )));
                  } else {
                    showDialog(
                      context: context,
                      builder: (context) => ErrorMessageDialog(
                          title: "Funcionalidade não implementada!",
                          text:
                              "Esta funcionalidade será implementada numa versão futura!"),
                    );
                  }
                },
              ),
            ),
            color: Colors.white,
          ),
          // Text input
          Flexible(
            child: Container(
              child: TextField(
                style: TextStyle(fontSize: 15.0),
                controller: textEditingController,
                decoration: InputDecoration.collapsed(
                  hintText: 'Escreva aqui',
                  hintStyle: TextStyle(color: Colors.grey),
                ),
              ),
            ),
          ),

          Material(
            child: new Container(
              margin: new EdgeInsets.symmetric(horizontal: 8.0),
              child: new IconButton(
                icon: new Icon(Icons.send),
                onPressed: () async {
                  final vm =
                      Provider.of<MessageListViewModel>(context, listen: false);
                  String message = textEditingController.text;
                  textEditingController.clear();
                  vm.addMessage(message, widget.chatId, widget.userId);
                  await chatInstance.sendMessage(message);
                  setState(() {});
                  scrollToBottom();
                },
                color: color,
              ),
            ),
            color: Colors.white,
          ),
        ],
      ),
      width: double.infinity,
      height: 50.0,
      decoration: new BoxDecoration(
          border:
              new Border(top: new BorderSide(color: Colors.grey, width: 0.5)),
          color: Colors.white),
    );
  }

  void getDataFromService() {
    final vm = Provider.of<MessageListViewModel>(context, listen: false);
    vm.fetchMessages(widget.chatId).then((value) async {
      messageList = vm.messageList;
      await chatInstance.createConnection(widget.otherUserId);
      chatInstance.connection.on('ReceivePrivateMessage', (message) {
        if (message[1] as int == widget.otherUserId) {
          vm.addMessage(
              message[0] as String, widget.chatId, widget.otherUserId);
        }
        setState(() {});
        scrollToBottom();
      });
      setState(() {});
      scrollToBottom();
    });
  }

  void scrollToBottom() {
    _controller.animateTo(
      _controller.position.maxScrollExtent,
      duration: Duration(seconds: 1),
      curve: Curves.fastOutSlowIn,
    );
  }

  @override
  void dispose() {
    internetConnection.cancel();
    chatInstance.stop();
    super.dispose();
  }
}
