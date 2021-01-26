class Message {
  final int id;
  final int chatId;
  final String messageSend;
  final int senderId;
  final DateTime time;

  Message({this.id, this.chatId, this.messageSend, this.senderId, this.time});

  factory Message.fromJson(Map<String, dynamic> json) {
    return Message(
      id: int.parse(json["id"].toString()),
      chatId: int.parse(json["chatId"].toString()),
      messageSend: json["messageSend"],
      senderId: int.parse(json["senderId"].toString()),
      time: DateTime.parse(json["time"].toString()),
    );
  }
}
