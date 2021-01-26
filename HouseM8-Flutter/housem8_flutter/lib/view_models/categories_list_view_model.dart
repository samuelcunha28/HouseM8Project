import 'dart:async';

import 'package:flutter/cupertino.dart';
import 'package:housem8_flutter/models/work_categories.dart';
import 'package:housem8_flutter/services/work_categories_service.dart';
import 'package:housem8_flutter/view_models/work_categories_view_model.dart';

class CategoriesListViewModel extends ChangeNotifier {
  List<WorkCategoriesViewModel> list = List<WorkCategoriesViewModel>();

  Future<void> fetchWorkCategories() async {
    final returned = await WorkCategoriesService().fetchWorkCategories();
    this.list =
        returned.map((e) => WorkCategoriesViewModel(workCategory: e)).toList();
    notifyListeners();
  }

  Future<void> deleteCategory(int index) async {
    WorkCategories categoryRemoved = list[index].workCategory;
    list.removeAt(index);
    notifyListeners();
    await WorkCategoriesService().deleteCategory(categoryRemoved);
  }

  Future<void> addCategory(WorkCategories workCategory) async {
    WorkCategoriesViewModel newValue =
        WorkCategoriesViewModel(workCategory: workCategory);
    list.add(newValue);
    notifyListeners();
    await WorkCategoriesService().addCategory(workCategory);
  }
}
