import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/work.dart';
import 'package:http/http.dart' as http;

class WorkService {
  Future<void> createWork(Work work) async {
    final url = DotEnv().env['REST_API_URL'] + "work/create";

    final String token = await StorageHelper.readToken();

    final response = await http.post(url,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode(<String, dynamic>{
          'date': work.date.toIso8601String(),
          'mate': work.mateId,
          'jobPost': work.jobPostId
        }));

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }

  Future<void> markJobAsDone(int jobId) async {
    final url = DotEnv().env['REST_API_URL'] + "work/markJobAsCompleted/$jobId";

    final String token = await StorageHelper.readToken();

    final response = await http.post(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode != 200) {
      throw Exception("Request Failed");
    }
  }

  Future<bool> isJobMarkedAsDone(int jobId) async {
    final String token = await StorageHelper.readToken();

    final url = DotEnv().env['REST_API_URL'] + "Work/find/$jobId";

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      return body['finishedConfirmedByMate'] as bool;
    } else {
      return false;
    }
  }
}
