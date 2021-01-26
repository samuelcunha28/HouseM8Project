import 'package:housem8_flutter/models/work_categories.dart';

class WorkCategoriesViewModel {
  WorkCategories workCategory = new WorkCategories();

  WorkCategoriesViewModel({this.workCategory});

  String get category {
    return this.workCategory.category.toString();
  }
}
