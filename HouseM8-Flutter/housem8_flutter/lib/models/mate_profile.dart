class MateProfile {
  final int id;
  final String username;
  final String firstName;
  final String lastName;
  final String email;
  final String address;
  String description;
  double averageRating;

  MateProfile.withDescription(
      {this.id,
      this.username,
      this.firstName,
      this.lastName,
      this.email,
      this.address,
      this.description,
      this.averageRating});

  MateProfile(
      {this.id,
      this.username,
      this.firstName,
      this.lastName,
      this.email,
      this.address,
      this.averageRating});

  factory MateProfile.fromJson(Map<String, dynamic> json) {
    if (json["description"] != null) {
      return MateProfile.withDescription(
        id: json["id"],
        username: json["userName"],
        firstName: json["firstName"],
        lastName: json["lastName"],
        email: json["email"],
        address: json["address"],
        description: json["description"],
        averageRating: json["averageRating"],
      );
    } else {
      return MateProfile(
        id: json["id"],
        username: json["userName"],
        firstName: json["firstName"],
        lastName: json["lastName"],
        email: json["email"],
        address: json["address"],
        averageRating: json["averageRating"],
      );
    }
  }
}
