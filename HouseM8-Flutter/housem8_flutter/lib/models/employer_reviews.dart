class EmployerReviews {
  final double rating;

  EmployerReviews({this.rating});

  factory EmployerReviews.fromJson(Map<String, dynamic> json) {
    return EmployerReviews(rating: json["rating"].toDouble());
  }
}
