import 'package:housem8_flutter/models/chat.dart';

class ChatViewModel {
  final Chat chat;

  ChatViewModel({this.chat});

  int get chatId {
    return this.chat.chatId;
  }

  int get userId {
    return this.chat.userId;
  }
}
