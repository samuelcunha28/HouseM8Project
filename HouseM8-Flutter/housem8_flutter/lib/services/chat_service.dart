import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/chat.dart';
import 'package:housem8_flutter/models/message.dart';
import 'package:http/http.dart' as http;

class ChatService {
  Future<List<Chat>> fetchChats() async {
    final String token = await StorageHelper.readToken();

    final url = DotEnv().env['REST_API_URL'] + "Chat/getChatsFromUser";

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      final Iterable json = body;
      return json.map((chat) => Chat.fromJson(chat)).toList();
    } else {
      return List<Chat>();
    }
  }

  Future<String> fetchNameByID(int userId) async {
    final String token = await StorageHelper.readToken();

    final url = DotEnv().env['REST_API_URL'] + "Users/$userId";

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      return body["userName"].toString();
    } else {
      return "...";
    }
  }

  Future<String> fetchLastMessageByChatID(int chatId) async {
    final String token = await StorageHelper.readToken();

    final url = DotEnv().env['REST_API_URL'] + "Chat/lastMessage/$chatId";

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      return body["messageSend"].toString();
    } else {
      return "...";
    }
  }

  Future<List<Message>> fetchMessagesFromChat(int chatId) async {
    final String token = await StorageHelper.readToken();

    final url =
        DotEnv().env['REST_API_URL'] + "Chat/getMessagesFromChat/$chatId";

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      final Iterable json = body;
      return json.map((message) => Message.fromJson(message)).toList();
    } else {
      return List<Message>();
    }
  }

  Future<void> createChat(int matchId) async {
    final url = DotEnv().env['REST_API_URL'] + "Chat/createChat/$matchId";

    final String token = await StorageHelper.readToken();

    final response = await http.post(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }

  Future<String> fetchConnectionId(int userId) async {
    final String token = await StorageHelper.readToken();

    final url = DotEnv().env['REST_API_URL'] + "Chat/chatConnection/$userId";

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      return body['connection'].toString();
    } else {
      return null;
    }
  }
}
