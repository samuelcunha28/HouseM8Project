import 'package:enum_to_string/enum_to_string.dart';
import 'package:housem8_flutter/enums/categories.dart';
import 'package:housem8_flutter/enums/payment.dart';

class JobPost {
  final int id;
  final String title;
  final Categories category;
  final String description;
  final bool tradable;
  final double initialPrice;
  final String address;
  final int employerId;
  final List<Payment> paymentMethod;
  final int range;

  JobPost(
      {this.id,
      this.title,
      this.category,
      this.description,
      this.tradable,
      this.initialPrice,
      this.address,
      this.employerId,
      this.paymentMethod,
      this.range});

  factory JobPost.fromJson(Map<String, dynamic> json) {
    return JobPost(
      id: json["id"],
      title: json["title"],
      category: EnumToString.fromString(Categories.values, json["category"]),
      description: json["description"],
      tradable: json["tradable"],
      initialPrice: json["initialPrice"].toDouble(),
      address: json["address"],
      employerId: json["employerId"],
      paymentMethod: List<Payment>.from(json["paymentMethod"]
          .map((x) => EnumToString.fromString(Payment.values, x))),
      range: json["range"],
    );
  }
}
