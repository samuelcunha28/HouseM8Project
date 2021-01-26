import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:housem8_flutter/view_models/categories_list_view_model.dart';
import 'package:housem8_flutter/view_models/work_categories_view_model.dart';
import 'package:housem8_flutter/widgets/remove_alert_dialog.dart';
import 'package:provider/provider.dart';
import 'package:recase/recase.dart';

class CategoriesList extends StatefulWidget {
  final List<WorkCategoriesViewModel> category;

  CategoriesList([this.category]);

  @override
  _CategoriesListState createState() => _CategoriesListState(category);
}

class _CategoriesListState extends State<CategoriesList> {
  final List<WorkCategoriesViewModel> category;

  _CategoriesListState(this.category);

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
        itemCount: this.category.length,
        itemBuilder: (context, index) {
          final categories = this.category[index];

          return Card(
            child: ListTile(
              title: Text(
                new ReCase(EnumToString.convertToString(
                        categories.workCategory.category))
                    .titleCase,
                style: TextStyle(fontSize: 18.0, color: Color(0xFF2F4858)),
              ),
              trailing: new IconButton(
                  icon: Icon(Icons.close),
                  color: Color(0xFF2F4858),
                  iconSize: 30.0,
                  onPressed: () {
                    showDialog(
                        context: context,
                        builder: (context) {
                          return RemoveAlertDialog("Remover Categoria",
                              'Tem a certeza que pretende remover esta categoria?');
                        }).then((value) {
                      if (value == "Sim") {
                        final vm = Provider.of<CategoriesListViewModel>(context,
                            listen: false);
                        vm.deleteCategory(index);
                        setState(() {});
                      }
                    });
                  }),
            ),
          );
        });
  }
}
