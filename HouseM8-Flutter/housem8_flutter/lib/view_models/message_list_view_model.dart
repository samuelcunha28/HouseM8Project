import 'package:flutter/material.dart';
import 'package:housem8_flutter/models/message.dart';
import 'package:housem8_flutter/services/chat_service.dart';
import 'package:housem8_flutter/view_models/message_view_model.dart';

class MessageListViewModel extends ChangeNotifier {
  List<MessageViewModel> messageList = List<MessageViewModel>();

  Future<void> fetchMessages(int chatId) async {
    final returned = await ChatService().fetchMessagesFromChat(chatId);
    this.messageList =
        returned.map((e) => MessageViewModel(message: e)).toList();
    notifyListeners();
  }

  void addMessage(String message, int chatId, int senderId) {
    Message newMessage = Message(
        id: messageList.length + 1,
        chatId: chatId,
        time: DateTime.now(),
        messageSend: message,
        senderId: senderId);
    messageList.add(MessageViewModel(message: newMessage));
  }
}
