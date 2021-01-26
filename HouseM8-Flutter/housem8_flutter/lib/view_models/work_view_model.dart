import 'package:flutter/material.dart';
import 'package:housem8_flutter/models/work.dart';
import 'package:housem8_flutter/services/work_service.dart';

class WorkViewModel extends ChangeNotifier {
  Future<void> addWork(Work work) async {
    await WorkService().createWork(work);
  }
}
