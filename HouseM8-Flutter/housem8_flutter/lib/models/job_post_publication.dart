import 'package:enum_to_string/enum_to_string.dart';
import 'package:housem8_flutter/enums/categories.dart';
import 'package:housem8_flutter/enums/payment.dart';
import 'package:housem8_flutter/models/address.dart';

class JobPostPublication {
  int id;
  String title;
  Categories category;
  String description;
  bool tradable;
  double initialPrice;
  Address address;
  List<Payment> paymentMethod;
  int range;

  JobPostPublication(
      {this.id,
      this.title,
      this.category,
      this.description,
      this.tradable,
      this.initialPrice,
      this.address,
      this.paymentMethod,
      this.range});

  factory JobPostPublication.fromJson(Map<String, dynamic> json) {
    return JobPostPublication(
      id: json["id"],
      title: json["title"],
      category: EnumToString.fromString(Categories.values, json["category"]),
      description: json["description"],
      tradable: json["tradable"],
      initialPrice: json["initialPrice"].toDouble(),
      address: json["address"],
      paymentMethod: List<Payment>.from(json["paymentMethod"]
          .map((x) => EnumToString.fromString(Payment.values, x))),
      range: json["range"],
    );
  }
}
