import 'package:flutter/material.dart';
import 'package:housem8_flutter/services/chat_service.dart';

import 'chat_view_model.dart';

class ChatListViewModel extends ChangeNotifier {
  List<ChatViewModel> chatList = List<ChatViewModel>();

  Future<void> fetchChats() async {
    final returned = await ChatService().fetchChats();
    this.chatList = returned.map((e) => ChatViewModel(chat: e)).toList();
    notifyListeners();
  }

  Future<String> fetchUserName(int userId) async {
    return await ChatService().fetchNameByID(userId);
  }

  Future<String> fetchLastMessage(int chatId) async {
    return await ChatService().fetchLastMessageByChatID(chatId);
  }
}
