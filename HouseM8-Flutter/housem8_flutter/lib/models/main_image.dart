class MainImage {
  final String name;

  MainImage({this.name});

  factory MainImage.fromJson(Map<String, dynamic> json) {
    return MainImage(name: json["name"]);
  }
}
