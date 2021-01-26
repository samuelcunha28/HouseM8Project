import 'dart:io';

import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/services/chat_service.dart';
import 'package:http/io_client.dart';
import 'package:signalr_core/signalr_core.dart';

class MessagingHelper {
  var connection;
  int myId;
  int matchId;
  String myConnectionId;
  String otherConnectionId;

  Future<void> createConnection(int matchId) async {
    this.matchId = matchId;
    await ChatService().createChat(matchId);
    String jwtToken = await StorageHelper.readToken();
    myId = int.parse(await StorageHelper.readTokenID());

    connection = HubConnectionBuilder()
        .withUrl(
            'https://10.0.2.2:5001/chatHub',
            HttpConnectionOptions(
              accessTokenFactory: () => Future.value(jwtToken),
              client: IOClient(
                  HttpClient()..badCertificateCallback = (x, y, z) => true),
              logging: (level, message) => print(message),
            ))
        .build();

    await connection.start();
  }

  Future<void> sendMessage(String message) async {
    myConnectionId = await ChatService().fetchConnectionId(myId);
    otherConnectionId = await ChatService().fetchConnectionId(matchId);
    await connection.invoke('SendPrivateMessage', args: [
      otherConnectionId,
      myConnectionId,
      message,
      DateTime.now().toIso8601String(),
      matchId,
      myId
    ]);
  }

  Future<void> stop() async {
    await connection.stop();
  }
}
