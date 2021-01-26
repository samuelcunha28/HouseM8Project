import 'package:enum_to_string/enum_to_string.dart';
import 'package:housem8_flutter/enums/categories.dart';

class WorkCategories {
  final Categories category;

  WorkCategories({this.category});

  factory WorkCategories.fromJson(Map<String, dynamic> json) {
    return WorkCategories(
        category:
            EnumToString.fromString(Categories.values, json["categories"]));
  }

  Map<String, dynamic> toJson() {
    return {
      'categories': EnumToString.convertToString(category),
    };
  }
}
