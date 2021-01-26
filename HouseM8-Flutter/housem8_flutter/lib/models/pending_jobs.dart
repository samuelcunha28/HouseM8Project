import 'package:enum_to_string/enum_to_string.dart';
import 'package:housem8_flutter/enums/Categories.dart';

class PendingJobs {
  final int jobId;
  final String title;
  final Categories category;
  final String description;

  PendingJobs({this.jobId, this.title, this.category, this.description});

  factory PendingJobs.fromJson(Map<String, dynamic> json) {
    return PendingJobs(
      jobId: int.parse(json["jobId"].toString()),
      title: json["title"],
      category: EnumToString.fromString(Categories.values, json["category"]),
      description: json["descritpion"],
    );
  }
}
