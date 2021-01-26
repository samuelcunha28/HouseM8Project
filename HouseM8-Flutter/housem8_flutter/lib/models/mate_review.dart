class MateReviews {
  final String comment;
  final double rating;

  MateReviews({this.rating, this.comment});

  factory MateReviews.fromJson(Map<String, dynamic> json) {
    return MateReviews(
      rating: json["rating"].toDouble(),
      comment: json["comment"],
    );
  }
}
