import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class JobSearchCard extends StatelessWidget {
  final url;
  final title;
  final range;

  const JobSearchCard({Key key, this.url, this.title, this.range})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    double width = MediaQuery.of(context).size.width;
    double height = MediaQuery.of(context).size.height;
    return Container(
        width: width,
        height: height / 1.7,
        alignment: Alignment.center,
        decoration: BoxDecoration(
            shape: BoxShape.rectangle,
            image: DecorationImage(
                image: NetworkImage(url),
                onError: (errDetails, stack) {},
                fit: BoxFit.cover),
            borderRadius: BorderRadius.circular(15)),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.end,
          children: [
            Container(
                width: width,
                height: height / 10,
                decoration: BoxDecoration(
                    boxShadow: <BoxShadow>[
                      BoxShadow(
                          color: Colors.black54,
                          blurRadius: 2.0,
                          offset: Offset(0.0, 0.75))
                    ],
                    shape: BoxShape.rectangle,
                    color: Colors.white,
                    borderRadius: BorderRadius.circular(15)),
                child: ButtonTheme(
                  height: height / 10,
                  minWidth: width,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(15),
                  ),
                  child: RaisedButton(
                    padding: EdgeInsets.only(top: 12.0, left: 20.0),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.start,
                      crossAxisAlignment: CrossAxisAlignment.stretch,
                      children: [
                        Text(
                          '' + title,
                          style: TextStyle(
                              color: Colors.black,
                              fontWeight: FontWeight.bold,
                              fontSize: 17),
                        ),
                        SizedBox(
                          height: 5,
                        ),
                        range.toString().length > 48
                            ? Text(
                                '' + range.substring(0, 48) + "...",
                                style: TextStyle(
                                    color: Colors.black, fontSize: 12),
                              )
                            : Text(
                                range,
                                style: TextStyle(
                                    color: Colors.black, fontSize: 12),
                              ),
                      ],
                    ),
                    color: Colors.white,
                    onPressed: () {
                      print("Ver publicação!");
                    },
                  ),
                )),
          ],
        ));
  }
}
