import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/chat.dart';
import 'package:housem8_flutter/pages/chat_messaging_page.dart';
import 'package:housem8_flutter/view_models/chat_list_view_model.dart';
import 'package:housem8_flutter/view_models/message_list_view_model.dart';
import 'package:provider/provider.dart';

class ChatCard extends StatefulWidget {
  final Color color;
  final Chat chat;

  const ChatCard({Key key, this.color, this.chat}) : super(key: key);

  @override
  _ChatCardState createState() => _ChatCardState();
}

class _ChatCardState extends State<ChatCard> {
  Color color;
  int myId = 0;
  String myName = "...";
  String otherUserName = "...";
  String lastMessage = "...";
  bool allowClick = false;

  @override
  void initState() {
    super.initState();
    color = widget.color;

    //Verificar Conectividade
    ConnectionHelper.checkConnection().then((value) {
      if (value) {
        getDataFromService();
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 2.0,
      child: ListTile(
        leading: CircleAvatar(
          backgroundImage: NetworkImage(
              "https://icon-library.com/images/facebook-user-icon/facebook-user-icon-4.jpg"), // no matter how big it is, it won't overflow
        ),
        title: Text(
          otherUserName.length < 30
              ? otherUserName
              : otherUserName.substring(0, 30) + "...",
          style: TextStyle(
              fontSize: 16.0,
              color: Color(Colors.black.value),
              fontWeight: FontWeight.w700),
        ),
        subtitle: Text(
          lastMessage.length < 30
              ? lastMessage
              : lastMessage.substring(0, 30) + "...",
          style: TextStyle(fontSize: 14.0, color: Color(Colors.black.value)),
        ),
        trailing: Row(mainAxisSize: MainAxisSize.min, children: <Widget>[
          IconButton(
            icon: Icon(Icons.message),
            color: color,
            iconSize: 30,
            tooltip: 'Abrir Chat',
            onPressed: () async {
              if (allowClick) {
                await Navigator.of(context)
                    .push(
                  MaterialPageRoute(
                      builder: (context) => ChangeNotifierProvider(
                            create: (context) => MessageListViewModel(),
                            child: ChatMessagingPage(
                              chatId: widget.chat.chatId,
                              color: color,
                              userId: myId,
                              userName: myName,
                              otherUserId: widget.chat.userId,
                              otherUserName: otherUserName,
                            ),
                          )),
                )
                    .then((value) {
                  getDataFromService();
                });
              }
            },
          ),
        ]),
      ),
    );
  }

  void getDataFromService() {
    final vm = Provider.of<ChatListViewModel>(context, listen: false);
    vm.fetchUserName(widget.chat.userId).then((value) {
      otherUserName = value;
      setState(() {});
    });
    vm.fetchLastMessage(widget.chat.chatId).then((value) {
      lastMessage = value;
      setState(() {});
    });
    StorageHelper.readTokenID().then((value) {
      myId = int.parse(value);
      vm.fetchUserName(myId).then((value) {
        myName = value;
        allowClick = true;
        setState(() {});
      });
    });
  }
}
