class Reviews {
  final String author;
  final String comment;
  final double rating;

  Reviews({this.author, this.comment, this.rating});

  factory Reviews.fromJson(Map<String, dynamic> json) {
    return Reviews(
      author: json["author"],
      comment: json["comment"],
      rating: json["rating"].toDouble(),
    );
  }
}
