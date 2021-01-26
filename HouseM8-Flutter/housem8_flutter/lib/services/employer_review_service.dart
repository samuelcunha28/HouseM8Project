import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/employer_reviews.dart';
import 'package:http/http.dart' as https;

class EmployerReviewService {
  Future<int> createEmployerReview(int employerId,
      [EmployerReviews employerReview]) async {
    final url = DotEnv().env['REST_API_URL'] + "Reviews/employer/$employerId";

    final String token = await StorageHelper.readToken();

    final response = await https.post(url,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode(<String, dynamic>{
          'rating': employerReview.rating,
        }));

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }

    return response.statusCode;
  }
}
