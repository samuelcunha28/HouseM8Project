class Chat {
  final int userId;
  final int chatId;

  Chat({this.userId, this.chatId});

  factory Chat.fromJson(Map<String, dynamic> json) {
    return Chat(
        userId: int.parse(json["userId"].toString()),
        chatId: int.parse(json["chatId"].toString()));
  }
}
