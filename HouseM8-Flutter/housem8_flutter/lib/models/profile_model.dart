class ProfileModel {
  int id;
  String userName;
  String firstName;
  String lastName;
  String email;
  String address;
  String description;
  double averageRating;

  ProfileModel.withDescription(
      {this.id,
      this.userName,
      this.firstName,
      this.lastName,
      this.email,
      this.address,
      this.description,
      this.averageRating});

  ProfileModel(
      {this.id,
      this.userName,
      this.firstName,
      this.lastName,
      this.email,
      this.address,
      this.averageRating});

  factory ProfileModel.fromJson(Map<String, dynamic> json) {
    if (json["description"] != null) {
      return ProfileModel.withDescription(
        id: json["id"],
        userName: json["userName"],
        firstName: json["firstName"],
        lastName: json["lastName"],
        email: json["email"],
        address: json["address"],
        description: json["description"],
        averageRating: json["averageRating"].toDouble(),
      );
    } else {
      return ProfileModel(
        id: json["id"],
        userName: json["userName"],
        firstName: json["firstName"],
        lastName: json["lastName"],
        email: json["email"],
        address: json["address"],
        averageRating: json["averageRating"].toDouble(),
      );
    }
  }
}
