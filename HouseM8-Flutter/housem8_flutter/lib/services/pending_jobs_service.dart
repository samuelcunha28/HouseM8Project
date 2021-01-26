import 'dart:async';
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/models/pending_jobs.dart';
import 'package:http/http.dart' as http;

class PendingJobsService {
  Future<List<PendingJobs>> fetchJobPosts() async {
    final url = DotEnv().env['REST_API_URL'] + "mateprofile/pending";

    final String token = await StorageHelper.readToken();

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      final Iterable json = body;
      return json
          .map((pendingJob) => PendingJobs.fromJson(pendingJob))
          .toList();
    } else {
      return List<PendingJobs>();
    }
  }

  Future<List<PendingJobs>> fetchEmployerPendingJobs() async {
    final url = DotEnv().env['REST_API_URL'] + "employerprofile/pending";

    final String token = await StorageHelper.readToken();

    final response = await http.get(url, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });

    if (response.statusCode == 200) {
      final body = jsonDecode(response.body);
      final Iterable json = body;
      return json
          .map((pendingJob) => PendingJobs.fromJson(pendingJob))
          .toList();
    } else {
      return List<PendingJobs>();
    }
  }
}
