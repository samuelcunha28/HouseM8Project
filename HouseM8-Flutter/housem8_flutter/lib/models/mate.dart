import 'package:enum_to_string/enum_to_string.dart';
import 'package:housem8_flutter/enums/categories.dart';
import 'package:housem8_flutter/enums/rank.dart';

class Mate {
  final int id;
  final String userName;
  final String email;
  final String description;
  final String firstName;
  final String lastName;
  final String address;
  final double averageRating;
  final int range;
  final Rank rank;
  final List<Categories> categories;

  Mate(
      {this.id,
      this.userName,
      this.email,
      this.description,
      this.firstName,
      this.lastName,
      this.address,
      this.averageRating,
      this.range,
      this.rank,
      this.categories});

  factory Mate.fromJson(Map<String, dynamic> json) {
    return Mate(
        id: json['id'].toInt(),
        userName: json['userName'],
        email: json['email'],
        description: json['description'],
        firstName: json['firstName'],
        lastName: json['lastName'],
        address: json['address'],
        averageRating: json['averageRating'].toDouble(),
        range: json['range'].toInt(),
        rank: EnumToString.fromString(Rank.values, json['rank']),
        categories: List<Categories>.from(json["categories"]
            .map((x) => EnumToString.fromString(Categories.values, x))));
  }
}
