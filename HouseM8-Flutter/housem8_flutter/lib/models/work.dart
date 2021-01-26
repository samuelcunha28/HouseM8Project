class Work {
  int id;
  DateTime date;
  int mateId;
  int jobPostId;
  int employerId;

  Work({this.id, this.date, this.mateId, this.jobPostId, this.employerId});

  factory Work.fromJson(Map<String, dynamic> json) {
    return Work(
        id: json["id"].toInt(),
        date: json["date"],
        mateId: json["mate"],
        jobPostId: json["jobPost"],
        employerId: json["employer"]);
  }
}
