import 'package:housem8_flutter/models/message.dart';

class MessageViewModel {
  final Message message;

  MessageViewModel({this.message});

  int get id {
    return this.message.id;
  }

  int get chatId {
    return this.message.chatId;
  }

  String get messageSend {
    return this.message.messageSend;
  }

  int get senderId {
    return this.message.senderId;
  }

  DateTime get time {
    return this.message.time;
  }
}
