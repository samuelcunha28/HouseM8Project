class FavoriteMates {
  final String name;
  final String email;

  FavoriteMates({this.name, this.email});

  factory FavoriteMates.fromJson(Map<String, dynamic> json) {
    return FavoriteMates(
      name: json["userName"],
      email: json["email"],
    );
  }
}
