import 'dart:convert';

import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/enums/payment.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:http/http.dart' as http;

class PaymentService {
  Future<String> makePayment(int jobId, double price) async {
    final url = DotEnv().env['REST_API_URL'] + "payment/makePayment/$jobId";

    final String token = await StorageHelper.readToken();

    final response = await http.post(url,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode(<String, dynamic>{
          'value': price,
          'paymentType': EnumToString.convertToString(Payment.PAYPAL),
        }));

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      return body['link'] as String;
    } else {
      throw Exception("Request Failed");
    }
  }

  Future<double> fetchCurrentPrice(int jobId) async {
    final String token = await StorageHelper.readToken();

    final url = DotEnv().env['REST_API_URL'] + "Work/find/$jobId";

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      return body['jobPost']['initialPrice'].toDouble();
    } else {
      throw Exception("Request Failed");
    }
  }
}
